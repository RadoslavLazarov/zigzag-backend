using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Security;
using System.Text.Json;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Configurations;
using ZigZag.Domain.Entities.Identity;
using ZigZag.Domain.Exceptions;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Infrastructure.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _cache;

        public AuthorizationService(
            IOptionsMonitor<JwtConfiguration> optionsMonitor,
            UserManager<UserEntity> userManager,
            IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cache)
        {
            _jwtConfiguration = optionsMonitor.CurrentValue;
            _userManager = userManager;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<AuthenticationModel> SignIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ApplicationException("Missing credentials");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User does not exists");
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                throw new ValidationException("Wrong password");
            }

            var tokens = await _jwtService.GenerateTokens(user);

            return new AuthenticationModel()
            {
                Email = user.Email,
                Roles = user.Roles,
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };
        }

        public async Task<SignOutModel> SignOut()
        {
            var user = _httpContextAccessor.HttpContext?.Items["User"] as CurrentUserModel;
            var accessToken = _httpContextAccessor.HttpContext?.Items["AccessToken"] as string;

            if (user == null || string.IsNullOrEmpty(accessToken))
            {
                throw new NotAuthorizedException();
            }

            var serializedUserModel = await _cache.GetStringAsync(accessToken);
            if (string.IsNullOrEmpty(serializedUserModel))
            {
                throw new NotAuthorizedException();
            }
            var userAuthorizedCacheModel = JsonSerializer.Deserialize<UserAuthorizedCacheModel>(serializedUserModel);

            await _jwtService.RemoveTokens(accessToken, userAuthorizedCacheModel.RefreshToken);

            return new SignOutModel() { Id = user.Id };
        }

        public async Task<TokenResponseModel> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException();
            }

            var serializedUserModel = await _cache.GetStringAsync(refreshToken);
            if (string.IsNullOrEmpty(serializedUserModel))
            {
                throw new NotAuthorizedException();
            }

            var userAuthorizedCacheModel = JsonSerializer.Deserialize<UserAuthorizedCacheModel>(serializedUserModel);

            var user = await _userManager.FindByIdAsync(userAuthorizedCacheModel.Id.ToString());
            if (user == null)
            {
                throw new NotAuthorizedException();
            }

            await _jwtService.RemoveTokens(userAuthorizedCacheModel.AccessToken, userAuthorizedCacheModel.RefreshToken);

            return await _jwtService.GenerateTokens(user);
        }

        public async Task<AuthenticationModel> Me(string refreshToken)
        {
            var user = _httpContextAccessor.HttpContext?.Items["User"] as CurrentUserModel;
            var accessToken = _httpContextAccessor.HttpContext?.Items["AccessToken"] as string;

            if (user == null || string.IsNullOrEmpty(accessToken))
            {
                throw new NotAuthorizedException();
            }

            var userId = _jwtService.ValidateAndGetUserId(accessToken, _jwtConfiguration.Secret);

            var result = new AuthenticationModel
            {
                Email = user.Username,
                Roles = user.Roles.ToList(),
            };

            if (userId == null)
            {
                if (string.IsNullOrEmpty(refreshToken))
                {
                    throw new NotAuthorizedException();
                }
                var userEntity = await _userManager.FindByIdAsync(user.Id);
                var tokens = await _jwtService.RefreshToken(refreshToken, userEntity);

                if (tokens == null)
                {
                    throw new NotAuthorizedException();
                }

                result.AccessToken = tokens.AccessToken;
                result.RefreshToken = tokens.RefreshToken;
                return result;
            }

            result.AccessToken = accessToken;
            result.RefreshToken = refreshToken;

            return result;
        }
    }
}

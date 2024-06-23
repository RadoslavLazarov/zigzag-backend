using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Entities.Identity;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using ZigZag.Domain.Configurations;
using System.IdentityModel.Tokens.Jwt;
using ZigZag.Domain.Models.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using MongoDB.Bson;
using System.Security;
using ZigZag.Domain.Exceptions;

namespace ZigZag.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IDistributedCache _cache;

        public JwtService(
            IOptionsMonitor<JwtConfiguration> optionsMonitor,
            IDistributedCache cache)
        {
            this._jwtConfiguration = optionsMonitor.CurrentValue;
            this._cache = cache;
        }

        public async Task<TokenResponseModel> GenerateTokens(UserEntity user)
        {
            var jwtToken = Generate(user.Id, user.Roles, _jwtConfiguration.Secret, _jwtConfiguration.AccessTokenTTL);
            var refreshToken = GenerateRefreshToken(256);

            var cacheModel = JsonSerializer.Serialize(new UserAuthorizedCacheModel
            {
                Id = user.Id.ToString(),
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            });

            await _cache.SetStringAsync(jwtToken, cacheModel, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_jwtConfiguration.AccessTokenTTL)
            });

            await _cache.SetStringAsync(refreshToken, cacheModel, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_jwtConfiguration.RefreshTokenTTL)
            });

            return new TokenResponseModel
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenResponseModel> RefreshToken(string refreshToken, UserEntity user)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException();
            }

            var serializedUserModel = await _cache.GetStringAsync(refreshToken);
            if (!string.IsNullOrEmpty(serializedUserModel))
            {
                throw new NotAuthorizedException();
            }
            var userAuthorizedCacheModel = JsonSerializer.Deserialize<UserAuthorizedCacheModel>(serializedUserModel);

            //await _cache.SetStringAsync(refreshToken.Token, JsonSerializer.Serialize(user), new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_jwtConfiguration.RefreshTokenTTL)
            //});
            await RemoveTokens(userAuthorizedCacheModel.AccessToken, userAuthorizedCacheModel.RefreshToken);

            return await this.GenerateTokens(user);
        }

        public ObjectId? ValidateAndGetUserId(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "id");
                if (userIdClaim != null && ObjectId.TryParse(userIdClaim.Value, out ObjectId userId))
                {
                    return userId;
                }
            }
            catch
            {
                // Log or handle validation errors
            }

            return null;
        }

        public async Task RemoveTokens(string accessToken, string refreshToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                await _cache.RemoveAsync(accessToken);
            }

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                await _cache.RemoveAsync(refreshToken);
            }
        }

        private static string Generate(ObjectId userId, IEnumerable<string> roles, string secret, int expirationInSeconds)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddSeconds(expirationInSeconds),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", userId.ToString()),
                    new Claim("roles", roles != null ? JsonSerializer.Serialize(roles) : null)
                }),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private static string GenerateRefreshToken(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

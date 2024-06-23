using HotChocolate.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System.Text.Json;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Configurations;
using ZigZag.Domain.Constants;
using ZigZag.Domain.Entities.Identity;
using ZigZag.Domain.Exceptions;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Infrastructure.Middlewares
{
    public class AuthorizationMiddleware : IMiddleware
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IDistributedCache _cache;

        public AuthorizationMiddleware(
            IUserService userService,
            IJwtService jwtService,
            IOptionsMonitor<JwtConfiguration> optionsMonitor,
            IDistributedCache cache)
        {
            _userService = userService;
            _jwtService = jwtService;
            _jwtConfiguration = optionsMonitor.CurrentValue;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //var userId = _jwtService.ValidateAccessToken(token);

            //if (!string.IsNullOrEmpty(userId))
            //{
            //    var user = await _userManager.FindByIdAsync(userId); // TODO: get from cache
            //    if (user != null)
            //    {
            //        context.Items["User"] = user;
            //        context.Items["AccessToken"] = token;
            //    }
            //}

            var tokenHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(tokenHeader))
            {
                var tokenParts = tokenHeader.Split(' ');
                if (tokenParts == null || tokenParts.Length <= 0)
                {
                    //Schema does not match
                    throw new ValidationException("Schema mismatch");
                }

                var token = tokenHeader.Split(' ').Last();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    var userId = _jwtService.ValidateAndGetUserId(token, _jwtConfiguration.Secret);
                    if (userId.HasValue)
                    {
                        await SetUserInContext(context, userId.Value, token);
                        //var serializedUserModel = await _cache.GetStringAsync(token);
                        //var userAuthorizedCacheModel = !string.IsNullOrEmpty(serializedUserModel) ? JsonSerializer.Deserialize<UserAuthorizedCacheModel>(serializedUserModel) : null;

                        //// If a user model is found in the cache, use it.
                        //// Otherwise, fetch it using userId if available.
                        //if (userAuthorizedCacheModel != null)
                        //{
                        //    await SetUserInContext(context, userAuthorizedCacheModel.Id, token);
                        //}
                        //else if (userId.HasValue)
                        //{
                        //    await SetUserInContext(context, userId.Value, token);
                        //}
                    }
                }
            }

            await next(context);
        }

        private async Task SetUserInContext(HttpContext context, ObjectId userId, string token)
        {
            var currentUser = await _userService.GetCurrentUserById(userId.ToString());
            if (currentUser != null)
            {
                context.Items["User"] = currentUser;
                context.Items["AccessToken"] = token;
            }
        }
    }
}

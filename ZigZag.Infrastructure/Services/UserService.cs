using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Constants;
using ZigZag.Domain.Entities.Identity;
using ZigZag.Domain.Exceptions;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IDistributedCache _cache;

        public UserService(
            UserManager<UserEntity> userManager,
            IDistributedCache cache)
        {
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<CurrentUserModel> GetCurrentUserById(string id)
        {
            var userCacheKey = string.Format(CachingKeyConstants.CurrentUser, id);
            var serializedUserModel = await _cache.GetStringAsync(userCacheKey);
            var currentUser = !string.IsNullOrEmpty(serializedUserModel) ? JsonSerializer.Deserialize<CurrentUserModel>(serializedUserModel) : null;

            if (currentUser != null)
            {
                return currentUser;
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException();
            }

            currentUser = new CurrentUserModel()
             {
                 Id = user.Id.ToString(),
                 Username = user.Email,
                 Roles = user.Roles
             };


            await _cache.SetStringAsync(userCacheKey, JsonSerializer.Serialize(currentUser), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600)
            });

            return currentUser;
        }
    }
}

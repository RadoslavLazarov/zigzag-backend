using ZigZag.Domain.Entities.Identity;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<CurrentUserModel> GetCurrentUserById(string id);
    }
}

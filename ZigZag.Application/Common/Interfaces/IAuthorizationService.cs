using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Common.Interfaces
{
    public interface IAuthorizationService
    {
        Task<AuthenticationModel> SignIn(string email, string password);

        Task<SignOutModel> SignOut();

        Task<TokenResponseModel> RefreshToken(string refreshToken);

        Task<AuthenticationModel> Me(string RefreshToken);
    }
}

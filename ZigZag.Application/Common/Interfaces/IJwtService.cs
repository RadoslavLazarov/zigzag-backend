using MongoDB.Bson;
using ZigZag.Domain.Entities.Identity;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Common.Interfaces
{
    public interface IJwtService
    {
        Task<TokenResponseModel> GenerateTokens(UserEntity user);

        Task<TokenResponseModel> RefreshToken(string refreshToken, UserEntity user);

        Task RemoveTokens(string accessToken, string refreshToken);

        ObjectId? ValidateAndGetUserId(string token, string secret);
    }
}

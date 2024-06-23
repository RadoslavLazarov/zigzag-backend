using MediatR;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<TokenResponseModel>
    {
        public string RefreshToken { get; set; }
    }
}

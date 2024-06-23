using MediatR;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.Ме
{
    public class MeCommand : IRequest<AuthenticationModel>
    {
        public string RefreshToken { get; set; }
    };
}

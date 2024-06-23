using MediatR;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.SignIn
{
    public record SignInCommand : IRequest<AuthenticationModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    };
}

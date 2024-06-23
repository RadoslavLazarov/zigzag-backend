using MediatR;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.SignOut
{
    public record SignOutCommand : IRequest<SignOutModel>;
}

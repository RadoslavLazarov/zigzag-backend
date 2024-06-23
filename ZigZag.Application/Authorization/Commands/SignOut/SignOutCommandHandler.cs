using MediatR;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.SignOut
{
    public class SignOutCommandHandler : IRequestHandler<SignOutCommand, SignOutModel>
    {
        private readonly IAuthorizationService _authorizationService;

        public SignOutCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<SignOutModel> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            return await _authorizationService.SignOut();
        }
    }
}

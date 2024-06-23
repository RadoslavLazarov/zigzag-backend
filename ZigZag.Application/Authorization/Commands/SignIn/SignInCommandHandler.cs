using MediatR;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthenticationModel>
    {
        private readonly IAuthorizationService _authorizationService;

        public SignInCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        public async Task<AuthenticationModel> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            return await _authorizationService.SignIn(request.Email, request.Password);
        }
    }
}

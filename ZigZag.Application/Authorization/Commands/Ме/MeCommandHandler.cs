using MediatR;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.Ме
{
    public class MeCommandHandler : IRequestHandler<MeCommand, AuthenticationModel>
    {
        private readonly IAuthorizationService _authorizationService;

        public MeCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        public async Task<AuthenticationModel> Handle(MeCommand request, CancellationToken cancellationToken)
        {
            return await _authorizationService.Me(request.RefreshToken);
        }
    }
}

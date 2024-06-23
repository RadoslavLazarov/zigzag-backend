using MediatR;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseModel>
    {
        private readonly IAuthorizationService _authorizationService;

        public RefreshTokenCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<TokenResponseModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authorizationService.RefreshToken(request.RefreshToken);
        }
    }
}

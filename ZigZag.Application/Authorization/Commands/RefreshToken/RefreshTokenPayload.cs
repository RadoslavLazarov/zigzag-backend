using ZigZag.Domain.Common;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.RefreshToken
{
    public class RefreshTokenPayload : Payload
    {
        public RefreshTokenPayload(TokenResponseModel tokenResponseModel)
        {
            TokenResponseModel = tokenResponseModel;
        }

        public RefreshTokenPayload(UserError error)
            : base(new[] { error })
        {
        }

        public TokenResponseModel? TokenResponseModel { get; }
    }
}

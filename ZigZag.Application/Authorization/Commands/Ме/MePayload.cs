using ZigZag.Domain.Common;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.Ме
{
    public class MePayload : Payload
    {
        public MePayload(AuthenticationModel authenticationModel)
        {
            AuthenticationModel = authenticationModel;
        }

        public MePayload(UserError error)
            : base(new[] { error })
        {
        }

        public AuthenticationModel? AuthenticationModel { get; }
    }
}

using ZigZag.Domain.Common;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.SignIn
{
    public class SignInPayload : Payload
    {
        public SignInPayload(AuthenticationModel authenticationModel)
        {
            AuthenticationModel = authenticationModel;
        }

        public SignInPayload(UserError error)
            : base(new[] { error })
        {
        }

        public AuthenticationModel? AuthenticationModel { get; }
    }
}

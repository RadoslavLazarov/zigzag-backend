using ZigZag.Domain.Common;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Application.Authorization.Commands.SignOut
{
    public class SignOutPayload : Payload
    {
        public SignOutPayload(SignOutModel signOutModel)
        {
            SignOutModel = signOutModel;
        }

        public SignOutPayload(UserError error)
            : base(new[] { error })
        {
        }

        public SignOutModel? SignOutModel { get; }
    }
}

using MediatR;
using ZigZag.Application.Authorization.Commands.RefreshToken;
using ZigZag.Application.Authorization.Commands.SignIn;
using ZigZag.Application.Authorization.Commands.SignOut;
using ZigZag.Application.Authorization.Commands.Ме;
using ZigZag.Domain.Common;

namespace ZigZag.WebAPI.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class AuthorizationMutations
    {
        public async Task<SignInPayload> SignInAsync(
            SignInCommand input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(input.Email))
            {
                return new SignInPayload(new UserError("Email cannot be empty.", "EMAIL_EMPTY"));
            }

            if (string.IsNullOrEmpty(input.Password))
            {
                return new SignInPayload(new UserError("Password cannot be empty.", "PASSWORD_EMPTY"));
            }

            var authenticationModel = await mediator.Send(input, cancellationToken);
            return new SignInPayload(authenticationModel);
        }

        public async Task<MePayload> MeAsync(
            MeCommand input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var authenticationModel = await mediator.Send(input, cancellationToken);
            return new MePayload(authenticationModel);
        }

        public async Task<SignOutPayload> SignOut(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return new SignOutPayload(await mediator.Send(new SignOutCommand(), cancellationToken));
        }

        public async Task<RefreshTokenPayload> RefreshToken(
            RefreshTokenCommand input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(input.RefreshToken))
            {
                return new RefreshTokenPayload(new UserError("RefreshToken cannot be empty.", "REFRESHTOKEN_EMPTY"));
            }

            var tokenResponseModel = await mediator.Send(input, cancellationToken);

            return new RefreshTokenPayload(tokenResponseModel);
        }
    }
}

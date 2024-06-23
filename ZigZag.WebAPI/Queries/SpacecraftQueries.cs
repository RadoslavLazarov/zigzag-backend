using MediatR;
using ZigZag.Application.Spacecrafts.Queries.GetSpacecrafts;
using ZigZag.Application.Venues.Queries.GetVenueByCategoryId;
using ZigZag.Domain.Models.Spacecraft;

namespace ZigZag.WebAPI.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class SpacecraftQueries
    {
        public async Task<SpacecraftsModel> GetSpacecraftsAsync(
        GetSpacecraftQuery input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        {
            return await mediator.Send(input, cancellationToken);
        }
    }
}

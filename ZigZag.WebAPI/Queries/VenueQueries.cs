using MediatR;
using ZigZag.Application.Venues.Queries.GetVenueByCategoryId;
using ZigZag.Application.Venues.Queries.GetVenueCategories;
using ZigZag.Domain.Entities.Venue;

namespace ZigZag.WebAPI.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class VenueQueries
    {
        public async Task<IQueryable<VenueCategoryEntity>> GetVenueCategoriesAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetVenueCategoriesQuery(), cancellationToken);
        }

        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100)]
        public async Task<IQueryable<VenueEntity>> GetVenuesByCategoryIdAsync(
        GetVenueByCategoryIdQuery input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        {
            return await mediator.Send(input, cancellationToken);
        }
    }
}

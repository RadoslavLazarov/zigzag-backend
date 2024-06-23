using MediatR;
using ZigZag.Domain.Entities.Venue;

namespace ZigZag.Application.Venues.Queries.GetVenueByCategoryId
{
    public record GetVenueByCategoryIdQuery([ID(nameof(VenueEntity))] string CategoryId) : IRequest<IQueryable<VenueEntity>>;
}

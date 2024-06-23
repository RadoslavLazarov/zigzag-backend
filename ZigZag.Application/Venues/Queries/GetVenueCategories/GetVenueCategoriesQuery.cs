using MediatR;
using ZigZag.Domain.Entities.Venue;

namespace ZigZag.Application.Venues.Queries.GetVenueCategories
{
    public record GetVenueCategoriesQuery : IRequest<IQueryable<VenueCategoryEntity>>;
}

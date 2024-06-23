using System.Linq.Expressions;
using ZigZag.Domain.Entities.Venue;

namespace ZigZag.Application.Common.Interfaces
{
    public interface IVenueRepository
    {
        Task<IQueryable<VenueEntity>> FindByAsync(Expression<Func<VenueEntity, bool>> match);
    }
}

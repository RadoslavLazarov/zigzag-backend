using MongoDB.Driver;
using System.Linq.Expressions;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Entities.Venue;

namespace ZigZag.Infrastructure.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly IMongoCollection<VenueEntity> _collection;

        public VenueRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<VenueEntity>("Venues");
        }

        public IQueryable<VenueEntity> GetAll()
        {
            return _collection.AsQueryable();
        }

        public virtual async Task<IQueryable<VenueEntity>> FindByAsync(Expression<Func<VenueEntity, bool>> match)
        {
            return _collection.AsQueryable().Where(match);
        }
    }
}

using MongoDB.Driver;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Entities.Venue;

namespace ZigZag.Infrastructure.Repositories
{
    public class VenueCategoryRepository : IVenueCategoryRepository
    {
        private readonly IMongoCollection<VenueCategoryEntity> _collection;

        public VenueCategoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<VenueCategoryEntity>("VenueCategories");
        }

        public IQueryable<VenueCategoryEntity> GetAll()
        {
            return _collection.AsQueryable();
        }
    }
}

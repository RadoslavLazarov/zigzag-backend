using ZigZag.Domain.Entities.Venue;

namespace ZigZag.Application.Common.Interfaces
{
    public interface IVenueCategoryRepository
    {
        IQueryable<VenueCategoryEntity> GetAll();
    }
}

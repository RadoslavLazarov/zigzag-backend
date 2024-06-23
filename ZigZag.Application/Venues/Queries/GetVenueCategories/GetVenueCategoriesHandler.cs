using MediatR;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Entities.Venue;

namespace ZigZag.Application.Venues.Queries.GetVenueCategories
{
    public class GetVenueCategoriesHandler : IRequestHandler<GetVenueCategoriesQuery, IQueryable<VenueCategoryEntity>>
    {
        private readonly IVenueCategoryRepository _venueCategoryRepository;

        public GetVenueCategoriesHandler(IVenueCategoryRepository venueCategoryRepository)
        {
            _venueCategoryRepository = venueCategoryRepository;
        }

        public async Task<IQueryable<VenueCategoryEntity>> Handle(GetVenueCategoriesQuery request, CancellationToken cancellationToken)
        {
            return _venueCategoryRepository.GetAll();
        }
    }
}

using MediatR;
using MongoDB.Bson;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Entities.Venue;

namespace ZigZag.Application.Venues.Queries.GetVenueByCategoryId
{
    public class GetVenueByCategoryIdQueryHandler : IRequestHandler<GetVenueByCategoryIdQuery, IQueryable<VenueEntity>>
    {
        private readonly IVenueRepository _venueRepository;

        public GetVenueByCategoryIdQueryHandler(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        public async Task<IQueryable<VenueEntity>> Handle(GetVenueByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            return await _venueRepository.FindByAsync(x => x.VenueCategoryId == ObjectId.Parse(request.CategoryId));
        }
    }
}

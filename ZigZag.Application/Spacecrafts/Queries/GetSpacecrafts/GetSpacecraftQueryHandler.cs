using MediatR;
using ZigZag.Application.Common.Interfaces;
using ZigZag.Domain.Models.Spacecraft;

namespace ZigZag.Application.Spacecrafts.Queries.GetSpacecrafts
{
    public class GetSpacecraftsQueryHandler : IRequestHandler<GetSpacecraftQuery, SpacecraftsModel>
    {
        private readonly ISpacecraftService _spacecraftService;

        public GetSpacecraftsQueryHandler(ISpacecraftService spacecraftService)
        {
            _spacecraftService = spacecraftService;
        }

        public async Task<SpacecraftsModel> Handle(GetSpacecraftQuery request, CancellationToken cancellationToken)
        {
            return await _spacecraftService.GetAllSpacecrafts(request.PageNumber);
        }
    }
}

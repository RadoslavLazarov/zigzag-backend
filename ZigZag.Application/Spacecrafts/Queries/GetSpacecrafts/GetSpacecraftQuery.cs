using MediatR;
using ZigZag.Domain.Models.Spacecraft;

namespace ZigZag.Application.Spacecrafts.Queries.GetSpacecrafts
{
    public record GetSpacecraftQuery : IRequest<SpacecraftsModel>
    {
        public int? PageNumber { get; set; }
    };
}

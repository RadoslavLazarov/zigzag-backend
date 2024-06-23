using ZigZag.Domain.Models.Spacecraft;

namespace ZigZag.Application.Common.Interfaces
{
    public interface ISpacecraftService
    {
        Task<SpacecraftsModel> GetAllSpacecrafts(int? pageNumber);
    }
}

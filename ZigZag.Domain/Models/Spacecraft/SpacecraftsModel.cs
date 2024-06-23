using System.Text.Json.Serialization;

namespace ZigZag.Domain.Models.Spacecraft
{
    public class SpacecraftsModel
    {
        [JsonPropertyName("spacecrafts")]
        public List<SpacecraftModel> Spacecrafts { get; set; } = new();

        [JsonPropertyName("page")]
        public PageModel Page { get; set; }
    }
}

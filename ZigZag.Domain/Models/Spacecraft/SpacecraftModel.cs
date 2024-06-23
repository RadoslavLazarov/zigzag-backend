using System.Text.Json.Serialization;

namespace ZigZag.Domain.Models.Spacecraft
{
    public class SpacecraftModel
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}

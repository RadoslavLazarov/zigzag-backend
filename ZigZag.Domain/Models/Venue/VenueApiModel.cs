using System.Text.Json.Serialization;

namespace ZigZag.Domain.Models.Venue
{
    public class VenueApiModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("category")]
        public string CategoryName { get; set; }
    }
}

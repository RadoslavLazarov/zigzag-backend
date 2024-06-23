using System.Text.Json.Serialization;

namespace ZigZag.Domain.Models.Venue
{
    public class VenuesApiModel
    {
        [JsonPropertyName("venues")]
        public List<VenueApiModel> Venues { get; set; }
    }
}

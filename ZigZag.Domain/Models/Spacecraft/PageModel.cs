using System.Text.Json.Serialization;

namespace ZigZag.Domain.Models.Spacecraft
{
    public class PageModel
    {
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("numberOfElements")]
        public int NumberOfElements { get; set; }

        [JsonPropertyName("totalElements")]
        public int TotalElements { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("firstPage")]
        public bool FirstPage { get; set; }

        [JsonPropertyName("lastPage")]
        public bool LastPage { get; set; }
    }
}

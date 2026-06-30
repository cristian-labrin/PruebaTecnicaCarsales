using System.Text.Json.Serialization;

namespace RickAndMortyBff.Models.External
{
    public class RickAndMortyApiResponse<T>
    {
        [JsonPropertyName("info")]
        public ApiInfo Info { get; set; } = new();

        [JsonPropertyName("results")]
        public List<T> Results { get; set; } = [];
    }

    public class ApiInfo
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("prev")]
        public string? Prev { get; set; }
    }
}

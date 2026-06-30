using System.Text.Json.Serialization;

namespace RickAndMortyBff.Models.External
{
    public class EpisodioExternal
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("air_date")]
        public string AirDate { get; set; } = string.Empty;

        [JsonPropertyName("episode")]
        public string Episode { get; set; } = string.Empty;

        [JsonPropertyName("characters")]
        public List<string> Characters { get; set; } = [];
    }
}

using System.Text.Json.Serialization;

namespace RickAndMortyBff.Models.External
{
    public class CharacterExternal
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("species")]
        public string Species { get; set; } = string.Empty;

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("origin")]
        public NamedResourceExternal Origin { get; set; } = new();

        [JsonPropertyName("location")]
        public NamedResourceExternal Location { get; set; } = new();
    }

    public class NamedResourceExternal
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}

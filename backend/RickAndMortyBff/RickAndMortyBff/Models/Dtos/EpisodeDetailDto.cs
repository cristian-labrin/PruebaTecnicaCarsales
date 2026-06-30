namespace RickAndMortyBff.Models.Dtos
{
    public class EpisodeDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AirDate { get; set; } = string.Empty;
        public string Episode { get; set; } = string.Empty;
        public IEnumerable<CharacterSummaryDto> Characters { get; set; } = [];
    }
}

using RickAndMortyBff.Models.External;

namespace RickAndMortyBff.Infrastructure
{
    public interface IRickAndMortyClient
    {
        Task<RickAndMortyApiResponse<EpisodioExternal>> GetEpisodesAsync(int page, string? name, CancellationToken cancellationToken);
        Task<EpisodioExternal?> GetEpisodeByIdAsync(int id, CancellationToken cancellationToken);
        Task<IReadOnlyList<CharacterExternal>> GetCharactersByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
        Task<CharacterExternal?> GetCharacterByIdAsync(int id, CancellationToken cancellationToken);
    }
}

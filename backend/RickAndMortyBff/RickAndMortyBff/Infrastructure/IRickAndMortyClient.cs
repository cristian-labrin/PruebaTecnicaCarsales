using RickAndMortyBff.Models.External;

namespace RickAndMortyBff.Infrastructure
{
    public interface IRickAndMortyClient
    {
        Task<RickAndMortyApiResponse<EpisodioExternal>> GetEpisodesAsync(int page, CancellationToken cancellationToken);
    }
}

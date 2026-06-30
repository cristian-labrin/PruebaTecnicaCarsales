using RickAndMortyBff.Models.Dtos;

namespace RickAndMortyBff.Services
{
    public interface IEpisodeService
    {
        Task<PagedResponseDto<EpisodioDto>> GetEpisodesAsync(int page, CancellationToken cancellationToken);
    }
}

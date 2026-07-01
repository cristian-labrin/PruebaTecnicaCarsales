using RickAndMortyBff.Models.Dtos;

namespace RickAndMortyBff.Services
{
    public interface IEpisodeService
    {
        Task<PagedResponseDto<EpisodeDto>> GetEpisodesAsync(int page, string? name, CancellationToken cancellationToken);
        Task<EpisodeDetailDto?> GetEpisodeDetailAsync(int id, CancellationToken cancellationToken);
    }
}

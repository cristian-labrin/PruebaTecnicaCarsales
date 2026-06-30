using RickAndMortyBff.Infrastructure;
using RickAndMortyBff.Models.Dtos;

namespace RickAndMortyBff.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IRickAndMortyClient _client;

        public EpisodeService(IRickAndMortyClient client)
        {
            _client = client;
        }

        public async Task<PagedResponseDto<EpisodioDto>> GetEpisodesAsync(int page, CancellationToken cancellationToken)
        {
            var external = await _client.GetEpisodesAsync(page, cancellationToken);

            var items = external.Results.Select(e => new EpisodioDto
            {
                Id = e.Id,
                Name = e.Name,
                AirDate = e.AirDate,
                Episode = e.Episode
            });

            return new PagedResponseDto<EpisodioDto>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = external.Info.Pages,
                TotalItems = external.Info.Count,
                HasNext = external.Info.Next is not null,
                HasPrev = external.Info.Prev is not null
            };
        }
    }
}

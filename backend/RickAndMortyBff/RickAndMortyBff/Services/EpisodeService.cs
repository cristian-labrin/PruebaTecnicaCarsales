using RickAndMortyBff.Infrastructure;
using RickAndMortyBff.Models.Dtos;
using RickAndMortyBff.Models.External;

namespace RickAndMortyBff.Services
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IRickAndMortyClient _client;

        public EpisodeService(IRickAndMortyClient client)
        {
            _client = client;
        }

        public async Task<PagedResponseDto<EpisodioDto>> GetEpisodesAsync(int page, string? name, CancellationToken cancellationToken)
        {
            var external = await _client.GetEpisodesAsync(page, name, cancellationToken);

            var items = external.Results.Select(MapToEpisodeDto);

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

        public async Task<EpisodeDetailDto?> GetEpisodeDetailAsync(int id, CancellationToken cancellationToken)
        {
            var episode = await _client.GetEpisodeByIdAsync(id, cancellationToken);
            if (episode is null)
            {
                return null;
            }

            var characterIds = episode.Characters.Select(ExtractIdFromUrl).Where(x => x > 0);
            var characters = await _client.GetCharactersByIdsAsync(characterIds, cancellationToken);

            return new EpisodeDetailDto
            {
                Id = episode.Id,
                Name = episode.Name,
                AirDate = episode.AirDate,
                Episode = episode.Episode,
                Characters = characters.Select(c => new CharacterSummaryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
            };
        }

        private static EpisodioDto MapToEpisodeDto(EpisodioExternal e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            AirDate = e.AirDate,
            Episode = e.Episode
        };

        private static int ExtractIdFromUrl(string url)
        {
            var segment = url.Split('/').LastOrDefault();
            return int.TryParse(segment, out var id) ? id : 0;
        }
    }
}

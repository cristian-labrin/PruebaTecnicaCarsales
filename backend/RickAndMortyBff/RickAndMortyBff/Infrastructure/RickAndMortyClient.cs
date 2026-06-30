using RickAndMortyBff.Models.External;
using System.Net;
using System.Net.Http.Json;

namespace RickAndMortyBff.Infrastructure
{
    public class RickAndMortyClient : IRickAndMortyClient
    {
        private readonly HttpClient _httpClient;

        public RickAndMortyClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RickAndMortyApiResponse<EpisodioExternal>> GetEpisodesAsync(int page, string? name, CancellationToken cancellationToken)
        {
            var url = $"episode?page={page}";
            if (!string.IsNullOrWhiteSpace(name))
            {
                url += $"&name={Uri.EscapeDataString(name)}";
            }

            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new RickAndMortyApiResponse<EpisodioExternal>();
            }

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<RickAndMortyApiResponse<EpisodioExternal>>(cancellationToken);
            return data ?? new RickAndMortyApiResponse<EpisodioExternal>();
        }

        public async Task<EpisodioExternal?> GetEpisodeByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"episode/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EpisodioExternal>(cancellationToken);
        }

        public async Task<IReadOnlyList<CharacterExternal>> GetCharactersByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            var idList = ids.Distinct().ToList();
            if (idList.Count == 0)
            {
                return [];
            }

            var joinedIds = string.Join(",", idList);
            var response = await _httpClient.GetAsync($"character/{joinedIds}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return [];
            }

            response.EnsureSuccessStatusCode();

            // La API devuelve un objeto único si se pide un solo id, o un arreglo si son varios.
            if (idList.Count == 1)
            {
                var single = await response.Content.ReadFromJsonAsync<CharacterExternal>(cancellationToken);
                return single is null ? [] : [single];
            }

            var many = await response.Content.ReadFromJsonAsync<List<CharacterExternal>>(cancellationToken);
            return many ?? [];
        }

        public async Task<CharacterExternal?> GetCharacterByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"character/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CharacterExternal>(cancellationToken);
        }
    }
}

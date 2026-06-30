using System.Net.Http.Json;
using RickAndMortyBff.Models.External;

namespace RickAndMortyBff.Infrastructure
{
    public class RickAndMortyClient : IRickAndMortyClient
    {
        private readonly HttpClient _httpClient;

        public RickAndMortyClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RickAndMortyApiResponse<EpisodioExternal>> GetEpisodesAsync(int page, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"episode?page={page}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<RickAndMortyApiResponse<EpisodioExternal>>(cancellationToken);
            return data ?? new RickAndMortyApiResponse<EpisodioExternal>();
        }
    }
}

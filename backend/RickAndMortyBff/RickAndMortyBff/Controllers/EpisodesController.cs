using Microsoft.AspNetCore.Mvc;
using RickAndMortyBff.Models.Dtos;
using RickAndMortyBff.Services;

namespace RickAndMortyBff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodesController : Controller
    {
        private readonly IEpisodeService _episodeService;

        public EpisodesController(IEpisodeService episodeService)
        {
            _episodeService = episodeService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<EpisodioDto>>> GetEpisodes(
            [FromQuery] int page = 1,
            [FromQuery] string? name = null,
            CancellationToken cancellationToken = default)
        {
            if (page < 1)
            {
                return BadRequest("El número de página debe ser mayor o igual a 1.");
            }

            var result = await _episodeService.GetEpisodesAsync(page, name, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EpisodeDetailDto>> GetEpisodeDetail(
            int id,
            CancellationToken cancellationToken = default)
        {
            var episode = await _episodeService.GetEpisodeDetailAsync(id, cancellationToken);
            if (episode is null)
            {
                return NotFound($"No se encontró el episodio con id {id}.");
            }

            return Ok(episode);
        }
    }
}

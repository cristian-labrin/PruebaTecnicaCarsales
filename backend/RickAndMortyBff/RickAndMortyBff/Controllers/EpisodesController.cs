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
            CancellationToken cancellationToken = default)
        {
            if (page < 1)
            {
                return BadRequest("El número de página debe ser mayor o igual a 1.");
            }

            var result = await _episodeService.GetEpisodesAsync(page, cancellationToken);
            return Ok(result);
        }
    }
}

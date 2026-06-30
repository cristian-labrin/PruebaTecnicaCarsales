using Microsoft.AspNetCore.Mvc;
using RickAndMortyBff.Models.Dtos;
using RickAndMortyBff.Services;

namespace RickAndMortyBff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CharacterDto>> GetCharacter(
            int id,
            CancellationToken cancellationToken = default)
        {
            var character = await _characterService.GetCharacterByIdAsync(id, cancellationToken);
            if (character is null)
            {
                return NotFound($"No se encontró el personaje con id {id}.");
            }

            return Ok(character);
        }
    }
}

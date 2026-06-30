using RickAndMortyBff.Infrastructure;
using RickAndMortyBff.Models.Dtos;

namespace RickAndMortyBff.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IRickAndMortyClient _client;

        public CharacterService(IRickAndMortyClient client)
        {
            _client = client;
        }

        public async Task<CharacterDto?> GetCharacterByIdAsync(int id, CancellationToken cancellationToken)
        {
            var character = await _client.GetCharacterByIdAsync(id, cancellationToken);
            if (character is null)
            {
                return null;
            }

            return new CharacterDto
            {
                Id = character.Id,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Gender = character.Gender,
                Image = character.Image,
                Origin = character.Origin.Name,
                Location = character.Location.Name
            };
        }
    }
}

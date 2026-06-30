using RickAndMortyBff.Models.Dtos;

namespace RickAndMortyBff.Services
{
    public interface ICharacterService
    {
        Task<CharacterDto?> GetCharacterByIdAsync(int id, CancellationToken cancellationToken);
    }
}

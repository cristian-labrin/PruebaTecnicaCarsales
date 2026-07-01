using Moq;
using RickAndMortyBff.Infrastructure;
using RickAndMortyBff.Models.External;
using RickAndMortyBff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickAndMortyBff.Tests
{
    public class CharacterServiceTests
    {
        [Fact]
        public async Task GetCharacterByIdAsync_CuandoExiste_MapeaYAplanaDatos()
        {
            // Arrange
            var externalCharacter = new CharacterExternal
            {
                Id = 1,
                Name = "Rick Sanchez",
                Status = "Alive",
                Species = "Human",
                Gender = "Male",
                Image = "https://rickandmortyapi.com/api/character/avatar/1.jpeg",
                Origin = new NamedResourceExternal { Name = "Earth (C-137)" },
                Location = new NamedResourceExternal { Name = "Citadel of Ricks" }
            };

            var clientMock = new Mock<IRickAndMortyClient>();
            clientMock
                .Setup(c => c.GetCharacterByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(externalCharacter);

            var service = new CharacterService(clientMock.Object);

            // Act
            var result = await service.GetCharacterByIdAsync(1, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
            Assert.Equal("Rick Sanchez", result.Name);
            Assert.Equal("Alive", result.Status);
            Assert.Equal("Human", result.Species);
            Assert.Equal("Earth (C-137)", result.Origin);
            Assert.Equal("Citadel of Ricks", result.Location);
        }

        [Fact]
        public async Task GetCharacterByIdAsync_CuandoNoExiste_DevuelveNull()
        {
            // Arrange: el cliente devuelve null (personaje inexistente)
            var clientMock = new Mock<IRickAndMortyClient>();
            clientMock
                .Setup(c => c.GetCharacterByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CharacterExternal?)null);

            var service = new CharacterService(clientMock.Object);

            // Act
            var result = await service.GetCharacterByIdAsync(99999, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}

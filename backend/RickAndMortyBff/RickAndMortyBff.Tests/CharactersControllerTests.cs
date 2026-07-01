using Microsoft.AspNetCore.Mvc;
using Moq;
using RickAndMortyBff.Controllers;
using RickAndMortyBff.Models.Dtos;
using RickAndMortyBff.Services;
using Xunit;

namespace RickAndMortyBff.Tests
{
    public class CharactersControllerTests
    {
        [Fact]
        public async Task GetCharacter_CuandoExiste_DevuelveOk()
        {
            // Arrange
            var characterDto = new CharacterDto
            {
                Id = 1,
                Name = "Rick Sanchez",
                Status = "Alive",
                Species = "Human",
                Gender = "Male",
                Image = "https://rickandmortyapi.com/api/character/avatar/1.jpeg",
                Origin = "Earth (C-137)",
                Location = "Citadel of Ricks"
            };

            var serviceMock = new Mock<ICharacterService>();
            serviceMock
                .Setup(s => s.GetCharacterByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(characterDto);

            var controller = new CharactersController(serviceMock.Object);

            // Act
            var result = await controller.GetCharacter(1, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(characterDto, okResult.Value);
        }

        [Fact]
        public async Task GetCharacter_CuandoNoExiste_DevuelveNotFound()
        {
            // Arrange
            var serviceMock = new Mock<ICharacterService>();
            serviceMock
                .Setup(s => s.GetCharacterByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CharacterDto?)null);

            var controller = new CharactersController(serviceMock.Object);

            // Act
            var result = await controller.GetCharacter(99999, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}

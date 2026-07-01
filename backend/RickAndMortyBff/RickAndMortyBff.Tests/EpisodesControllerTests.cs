using Microsoft.AspNetCore.Mvc;
using Moq;
using RickAndMortyBff.Controllers;
using RickAndMortyBff.Models.Dtos;
using RickAndMortyBff.Services;
using Xunit;

namespace RickAndMortyBff.Tests
{
    public class EpisodesControllerTests
    {
        [Fact]
        public async Task GetEpisodes_ConPaginaValida_DevuelveOk()
        {
            // Arrange
            var pagedResponse = new PagedResponseDto<EpisodeDto>
            {
                Items = [new EpisodeDto { Id = 1, Name = "Pilot", AirDate = "December 2, 2013", Episode = "S01E01" }],
                CurrentPage = 1,
                TotalPages = 3,
                TotalItems = 51,
                HasNext = true,
                HasPrev = false
            };

            var serviceMock = new Mock<IEpisodeService>();
            serviceMock
                .Setup(s => s.GetEpisodesAsync(1, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResponse);

            var controller = new EpisodesController(serviceMock.Object);

            // Act
            var result = await controller.GetEpisodes(1, null, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(pagedResponse, okResult.Value);
        }

        [Fact]
        public async Task GetEpisodes_ConPaginaInvalida_DevuelveBadRequest()
        {
            // Arrange
            var serviceMock = new Mock<IEpisodeService>();
            var controller = new EpisodesController(serviceMock.Object);

            // Act
            var result = await controller.GetEpisodes(0, null, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetEpisodeDetail_CuandoNoExiste_DevuelveNotFound()
        {
            // Arrange: el service devuelve null (episodio inexistente)
            var serviceMock = new Mock<IEpisodeService>();
            serviceMock
                .Setup(s => s.GetEpisodeDetailAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((EpisodeDetailDto?)null);

            var controller = new EpisodesController(serviceMock.Object);

            // Act
            var result = await controller.GetEpisodeDetail(99999, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}

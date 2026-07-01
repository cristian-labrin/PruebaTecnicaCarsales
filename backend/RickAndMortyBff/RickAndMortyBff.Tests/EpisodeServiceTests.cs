using Moq;
using RickAndMortyBff.Infrastructure;
using RickAndMortyBff.Models.External;
using RickAndMortyBff.Services;
using Xunit;

namespace RickAndMortyBff.Tests
{
    public class EpisodeServiceTests
    {
        [Fact]
        public async Task GetEpisodesAsync_MapeaDatosYCalculaPaginacion_Correctamente()
        {
            // Arrange: preparamos un cliente falso que devuelve datos controlados
            var apiResponse = new RickAndMortyApiResponse<EpisodeExternal>
            {
                Info = new ApiInfo
                {
                    Count = 51,
                    Pages = 3,
                    Next = "https://rickandmortyapi.com/api/episode?page=2",
                    Prev = null
                },
                Results =
                [
                    new EpisodeExternal
                {
                    Id = 1,
                    Name = "Pilot",
                    AirDate = "December 2, 2013",
                    Episode = "S01E01"
                }
                ]
            };

            var clientMock = new Mock<IRickAndMortyClient>();
            clientMock
                .Setup(c => c.GetEpisodesAsync(1, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var service = new EpisodeService(clientMock.Object);

            // Act: ejecutamos el método que queremos probar
            var result = await service.GetEpisodesAsync(1, null, CancellationToken.None);

            // Assert: verificamos que el resultado es el esperado
            Assert.Single(result.Items);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(51, result.TotalItems);
            Assert.True(result.HasNext);
            Assert.False(result.HasPrev);

            var episode = result.Items.First();
            Assert.Equal("Pilot", episode.Name);
            Assert.Equal("S01E01", episode.Episode);
        }

        [Fact]
        public async Task GetEpisodesAsync_CuandoNoHayResultados_DevuelveListaVacia()
        {
            // Arrange: el cliente devuelve una respuesta vacía (sin episodios)
            var emptyResponse = new RickAndMortyApiResponse<EpisodeExternal>
            {
                Info = new ApiInfo { Count = 0, Pages = 0, Next = null, Prev = null },
                Results = []
            };

            var clientMock = new Mock<IRickAndMortyClient>();
            clientMock
                .Setup(c => c.GetEpisodesAsync(1, "noexiste", It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyResponse);

            var service = new EpisodeService(clientMock.Object);

            // Act
            var result = await service.GetEpisodesAsync(1, "noexiste", CancellationToken.None);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalItems);
            Assert.False(result.HasNext);
            Assert.False(result.HasPrev);
        }
    }
}

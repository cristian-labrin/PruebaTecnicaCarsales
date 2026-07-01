using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using RickAndMortyBff.Middleware;
using Xunit;

namespace RickAndMortyBff.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_CuandoFallaApiExterna_Devuelve502()
        {
            // Arrange: el "siguiente" en el pipeline lanza HttpRequestException
            RequestDelegate next = _ => throw new HttpRequestException("Fallo externo");
            var middleware = new ErrorHandlingMiddleware(next, NullLogger<ErrorHandlingMiddleware>.Instance);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadGateway, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);
        }

        [Fact]
        public async Task InvokeAsync_CuandoErrorInesperado_Devuelve500()
        {
            // Arrange: el "siguiente" lanza una excepción genérica
            RequestDelegate next = _ => throw new InvalidOperationException("Error inesperado");
            var middleware = new ErrorHandlingMiddleware(next, NullLogger<ErrorHandlingMiddleware>.Instance);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_CuandoNoHayError_NoModificaLaRespuesta()
        {
            // Arrange: el "siguiente" se ejecuta sin problemas
            RequestDelegate next = _ => Task.CompletedTask;
            var middleware = new ErrorHandlingMiddleware(next, NullLogger<ErrorHandlingMiddleware>.Instance);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert: el status queda en el valor por defecto (200), no lo tocó
            Assert.Equal(200, context.Response.StatusCode);
        }
    }
}

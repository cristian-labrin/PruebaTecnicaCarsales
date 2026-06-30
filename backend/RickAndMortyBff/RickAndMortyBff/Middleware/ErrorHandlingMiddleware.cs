using System.Net;
using System.Text.Json;

namespace RickAndMortyBff.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al comunicarse con la API externa.");
                await WriteErrorAsync(context, HttpStatusCode.BadGateway,
                    "No se pudo obtener la información desde el servicio externo.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado.");
                await WriteErrorAsync(context, HttpStatusCode.InternalServerError,
                    "Ocurrió un error inesperado. Intente nuevamente más tarde.");
            }
        }

        private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var payload = JsonSerializer.Serialize(new { error = message });
            await context.Response.WriteAsync(payload);
        }
    }
}

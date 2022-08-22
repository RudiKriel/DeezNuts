using System.Net;
using System.Text.Json;
using DeezNuts.Errors;

namespace DeezNuts.Helpers
{
    public class ExceptionHelper
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHelper> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHelper(RequestDelegate next, ILogger<ExceptionHelper> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() ? new APIException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) : new APIException(context.Response.StatusCode, "Internal Server Error");
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}

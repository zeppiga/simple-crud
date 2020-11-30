using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace simple_crud.Middleware
{
    public sealed class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Request Host: {context.Request.Host} Path: {context.Request.Path} QueryString: {context.Request.QueryString} Method: {context.Request.Method} \n Body: {await GetBody(context)}");
            await _next(context);
        }

        private static async Task<string> GetBody(HttpContext context)
        {
            context.Request.EnableBuffering();

            // TODO consider RecyclableMemoryStream as there could be problems when requests are large enough for loh
            await using var ms = new MemoryStream();
            await context.Request.Body.CopyToAsync(ms);

            ms.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(ms);
            var result = await reader.ReadToEndAsync();
            ms.Seek(0, SeekOrigin.Begin);
            context.Request.Body.Position = 0;

            return result;
        }
    }
}

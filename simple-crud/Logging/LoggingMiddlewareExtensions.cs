using Microsoft.AspNetCore.Builder;

namespace simple_crud.Logging
{
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
using Microsoft.AspNetCore.Builder;

namespace simple_crud.Middleware
{
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace simple_crud.Middleware
{
    public static class ExceptionHandler
    {
        public static void Handle(IApplicationBuilder errorApp)
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/html";

                await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandlerPathFeature?.Error is NotImplementedException)
                    await context.Response.WriteAsync("Feature not yet implemented!<br><br>\r\n");

                await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
                await context.Response.WriteAsync("</body></html>\r\n");
            });
        }
    }
}
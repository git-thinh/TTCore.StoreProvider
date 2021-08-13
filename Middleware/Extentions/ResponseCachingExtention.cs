using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace TTCore.StoreProvider.Middleware.Extentions
{
    public static class ResponseCachingExtention
    {
        public static void UseResponseCachingMiddleware(this IApplicationBuilder app)
        {
            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary]
                    = new string[] { "Accept-Encoding" };

                await next();
            });
        }
    }
}

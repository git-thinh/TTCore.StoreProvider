﻿using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;


//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync($"Hello {CultureInfo.CurrentCulture.DisplayName}");
//});

namespace TTCore.StoreProvider.Middleware
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(cultureQuery);

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
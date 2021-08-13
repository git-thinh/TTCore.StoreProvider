using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TTCore.StoreProvider.Middleware.Extentions
{
    public static class CorsMiddlewareExtention
    {
        public static void AddCorsPolicyService(this IServiceCollection services)
        {
            //services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            //{
            //    builder.AllowAnyMethod().AllowAnyHeader()
            //           .WithOrigins("http://localhost:44317")
            //           .AllowCredentials();
            //}));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowCredentials()
                            .WithOrigins("https://localhost:4200")
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }

        public static void UseCorsPolicyMiddleware(this IApplicationBuilder app)
        {
            app.UseCors("AllowAllOrigins");

            //app.UseCors("CorsPolicy");
            //// Global cors policy
            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());
        }
    }
}

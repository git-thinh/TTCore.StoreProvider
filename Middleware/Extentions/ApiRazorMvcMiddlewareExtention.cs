using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TTCore.StoreProvider.Middleware.Extentions
{
    public static class ApiRazorMvcMiddlewareExtention
    {
        public static void AddApiRazorMvcService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger.Api", Version = "v1" }); });

            services.AddRazorPages();
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default30", new CacheProfile() { Duration = 30 });
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllersWithViews();
        }

        public static void MapApiRazorMvcMiddleware(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();

            endpoints.MapRazorPages();

            //endpoints.MapControllerRoute(name: "blog",
            //            pattern: "blog/{*article}",
            //            defaults: new { controller = "Blog", action = "Article" });

            //endpoints.MapControllerRoute(name: "default",
            //            pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}

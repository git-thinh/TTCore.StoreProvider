using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using TTCore.StoreProvider.Middleware;

namespace TTCore.StoreProvider
{
    public class ApiRouteBaseAttribute : Attribute, IRouteTemplateProvider
    {
        public string Template => "api/[controller]";
        public int? Order => 2;
        //public string Name { get; set; }
        public string Name { get; set; } = "[controller]_[action]";
    }

    public static class ApiRazorMvcMiddleware
    {
        public static void AddApiRazorMvcService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger.Api", Version = "v1" }); });

            services.AddRazorPages();
            services.AddMvc(options => { options.SetMvcResponseCaching(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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

            //endpoints.MapAreaControllerRoute("zebra_route", "Zebra", "Manage/{controller}/{action}/{ id ?}");
            //endpoints.MapControllerRoute("default_route", "{controller}/{action}/{id?}");

            //endpoints.MapControllerRoute("Zebra_route", "Manage/{controller}/{action}/{id?}",
            //    defaults: new { area = "Zebra" }, constraints: new { area = "Zebra" });
            //endpoints.MapControllerRoute("default_route", "{controller}/{action}/{id?}");
        }
    }
}

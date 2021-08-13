using TTCore.StoreProvider.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TTCore.StoreProvider.Middleware;
using TTCore.StoreProvider.Middleware.Extentions;

namespace TTCore.StoreProvider
{
    public class Startup
    {
        IConfiguration _configuration { get; }
        IWebHostEnvironment _environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DbMemoryContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
            services.AddTransient<FactoryActivatedMiddleware>();

            services.AddCorsPolicyService();
            services.AddTransient<ValidateMimeMultipartContentFilter>();

            //services.AddResponseCaching();
            services.AddDirectoryBrowser();

            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger.Api", Version = "v1" }); });
            services.AddRazorPages();

            services.AddSignalRService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebsocketMiddleware();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AIT.UI.Api v1"));

            app.UseMiddleware<RequestCultureMiddleware>();
            app.UseMiddleware<FactoryActivatedMiddleware>();

            app.UseCorsPolicyMiddleware();
            //app.UseResponseCachingMiddleware();

            app.UseStaticFileMiddleware(env);

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSignalREndpointRoute();
                endpoints.Test_POSTStreamPipe_MapEndpointRoute();

                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

    }

}

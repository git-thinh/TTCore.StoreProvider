using TTCore.StoreProvider.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TTCore.StoreProvider.Middleware;
using TTCore.StoreProvider.Middleware.Extentions;
using TTCore.StoreProvider.Services;

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
            services.AddDbContext<DbMemoryContext>(options => options.UseInMemoryDatabase("DbRamEntity"));
            services.AddDbContext<DbUserContext>(options => options.UseInMemoryDatabase("DbRamUser"));

            services.AddTransient<FactoryActivatedMiddleware>();
            services.AddSingleton<CacheMemoryRuntime>();

            services.AddCorsPolicyService();
            services.AddTransient<ValidateMimeMultipartContentFilter>();

            services.AddResponseCaching();
            services.AddDirectoryBrowser();

            services.AddApiRazorMvcService();

            services.AddSignalRService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebsocketMiddleware();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            //app.UseExceptionHandler("/Home/Error");

            app.UseMiddleware<RequestCultureMiddleware>();
            app.UseMiddleware<FactoryActivatedMiddleware>();

            app.UseCorsPolicyMiddleware();
            app.UseResponseCachingMiddleware();

            app.UseStaticFileMiddleware(env);

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapApiRazorMvcMiddleware(app);

                endpoints.MapSignalREndpointRoute();
                endpoints.Test_POSTStreamPipe_MapEndpointRoute();
            });
        }
    }
}

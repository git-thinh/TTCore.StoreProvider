using TTCore.StoreProvider.Data;
using TTCore.StoreProvider.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.Extensions.FileProviders;
using TTCore.StoreProvider.Extentions;

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

            //--------------------------------------------------------

            app.UseMiddleware<RequestCultureMiddleware>();
            app.UseMiddleware<FactoryActivatedMiddleware>();

            //--------------------------------------------------------

            //app.UseFileServer();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "test")),
                RequestPath = "/test"
            });

            //--------------------------------------------------------

            app.UseCorsPolicyMiddleware();

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

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
using TTCore.StoreProvider.Models;
using TTCore.StoreProvider.ServiceBackground;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

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
            services.AddHostedService<GrpcService>();

            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
            //services.AddGrpc(o => { });
            //services.AddGrpcHttpApi(o =>
            //{
            //    o.JsonFormatter = new JsonFormatter(new JsonFormatter.Settings(formatDefaultValues: false));
            //    o.JsonParser = new JsonParser(new JsonParser.Settings(recursionLimit: 1));
            //});
            //services.AddMvc();

            ////services.AddGrpc();
            //////services.AddGrpcClient<Ticketer.TicketerClient>(c =>
            //////{
            //////    //var url = Configuration.GetServiceUri("backend");
            //////    var url = new Uri("http://localhost:42656");
            //////    c.Address = url;
            //////});
            //////services.AddSingleton<Greet.Greeter.GreeterClient>(services =>
            //////{
            //////    var channel = GrpcChannel.ForAddress("http://localhost:42656");
            //////    var client = new Greet.Greeter.GreeterClient(channel);
            //////    var reply = client.SayHello(new Greet.HelloRequest() { Name = "123" });
            //////    return client;
            //////});


            var _appSettings = new AppSettings();
            _configuration.GetSection("AppSettings").Bind(_appSettings);

            services.AddSingleton<AppSettings>(_appSettings);
            services.AddSingleton<IJwtService, JwtService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Name);
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var SecretBytes = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                var SecurityKey = new SymmetricSecurityKey(SecretBytes);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateActor = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = SecurityKey
                };
            });


            services.AddSingleton<IImageService, ImageService>();

            services.AddSingleton<RedisService>();
            services.AddSingleton<IHostedService>(p => p.GetService<RedisService>());

            services.Configure<CollectionItems<UserLogin>>(_configuration);
            services.Configure<CollectionItems<Article>>(_configuration);

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

            //services.AddGrpcSwagger();
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

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<HttpApiGreeterService>();
                //endpoints.MapGrpcService<GreeterService>();
                //endpoints.MapGrpcService<TicketerService>();

                endpoints.MapApiRazorMvcMiddleware(app);

                endpoints.MapSignalREndpointRoute();
                endpoints.Test_POSTStreamPipe_MapEndpointRoute();
            });
        }

    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TTCore.StoreProvider.ServiceBackground;

namespace TTCore.StoreProvider
{
    public class App
    {

        private static IServiceProvider _service = null;
        public static IServiceProvider Service { get { return _service; } }
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("App.Run(): Init main...");
                var host = CreateHostBuilder(args).Build();
                _service = host.Services;
                host.Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "App.Run(): Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                //.UseKestrel(options =>
                //{
                //    options.ListenLocalhost(5000);
                //    options.ListenLocalhost(5001);
                //    options.ListenLocalhost(5002);
                //})
                //.UseIISIntegration()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("wwwroot/json/user.json", optional: false, reloadOnChange: false);
                    config.AddJsonFile("wwwroot/json/article.json", optional: false, reloadOnChange: true);

                    string environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string file = Path.Combine(path, "appsettings.json");
                    if (File.Exists(file)) config.AddJsonFile(file, optional: true);

                    file = Path.Combine(path, string.Format("appsettings.{0}.json", environmentName));
                    if (File.Exists(file)) config.AddJsonFile(file, optional: true);

                    file = Path.Combine(path, string.Format("appsettings.{0}.Redis.json", environmentName));
                    if (File.Exists(file)) config.AddJsonFile(file, optional: true);

                    file = Path.Combine(path, string.Format("appsettings.{0}.ConnectionString.json", environmentName));
                    if (File.Exists(file)) config.AddJsonFile(file, optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Type typeStartup = typeof(Startup);
                    webBuilder.UseStartup(typeStartup);
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<RedisNotificationBroker>();
                    services.AddHostedService<LifetimeEventsHostedService>();
                    services.AddHostedService<TimedHostedService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog(); // NLog: Setup NLog for Dependency injection
            return host;
        }
    }
}

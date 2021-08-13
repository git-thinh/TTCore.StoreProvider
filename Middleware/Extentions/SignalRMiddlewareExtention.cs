using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TTCore.StoreProvider.Hubs;
using TTCore.StoreProvider.ServiceBackground;

namespace TTCore.StoreProvider.Middleware.Extentions
{
    public static class SignalRMiddlewareExtention
    {
        public static void AddSignalRService(this IServiceCollection services)
        {
            string redisConnect = "127.0.0.1:1000,allowAdmin=true,abortConnect=false,defaultDatabase=15,syncTimeout=5000";
            services.AddSignalR()
                .AddMessagePackProtocol(options =>
                {
                    //options.SerializerOptions = MessagePackSerializerOptions.Standard
                    //    //.WithResolver(new CustomResolver())
                    //    .WithSecurity(MessagePackSecurity.UntrustedData);
                })
                .AddStackExchangeRedis(redisConnect, options => {
                    options.Configuration.ChannelPrefix = "MyApp";
                });
            services.AddHostedService<ClockWorker>();
        }

        public static void MapSignalREndpointRoute(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<ChatHub>("/hubs/chat");
            endpoints.MapHub<ClockHub>("/hubs/clock");
            endpoints.MapHub<ImageMessageHub>("/hubs/image"); 
            endpoints.MapHub<GroupHub>("/hubs/group");
        }
    }
}

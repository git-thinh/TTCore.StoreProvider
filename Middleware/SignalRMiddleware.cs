using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TTCore.StoreProvider.Hubs;
using TTCore.StoreProvider.ServiceBackground;

namespace TTCore.StoreProvider.Middleware
{
    public static class SignalRMiddleware
    {
        public static void AddSignalRService(this IServiceCollection services)
        {
            services.AddSignalR()
                .AddMessagePackProtocol(options =>
                {
                    //options.SerializerOptions = MessagePackSerializerOptions.Standard
                    //    //.WithResolver(new CustomResolver())
                    //    .WithSecurity(MessagePackSecurity.UntrustedData);

                    //StaticCompositeResolver.Instance.Register(DynamicGenericResolver.Instance, StandardResolver.Instance);
                    //options.SerializerOptions = MessagePackSerializerOptions.Standard
                    //    .WithResolver(StaticCompositeResolver.Instance)
                    //    .WithSecurity(MessagePackSecurity.UntrustedData);
                });
                //.AddStackExchangeRedis(redisConnect, options =>
                //{
                //    options.Configuration.ChannelPrefix = "MyApp";
                //});
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

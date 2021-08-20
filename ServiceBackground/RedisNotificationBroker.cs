using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using TTCore.StoreProvider.Hubs;

namespace TTCore.StoreProvider.ServiceBackground
{
    public class RedisNotificationBroker : BackgroundService
    {
        readonly string redisConnect = "127.0.0.1:1000,allowAdmin=true,abortConnect=false,defaultDatabase=15,syncTimeout=5000";
        readonly IHubContext<RedisHub> _appHubContext;
        readonly ILogger _logger;
        readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        readonly string ServiceName = nameof(RedisNotificationBroker);

        static ConnectionMultiplexer connection = null;
        public RedisNotificationBroker(IHubContext<RedisHub> hubContext,
            ILoggerFactory loggerFactory)
        {
            _appHubContext = hubContext;
            _logger = loggerFactory.CreateLogger(GetType());
            connection = ConnectionMultiplexer.Connect(redisConnect);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} background task is stopping."));

            // https://redis.io/topics/notifications
            // $ redis-cli      > config set notify - keyspace - events KEA
            // $ redis-cli--csv > psubscribe '__key*__:*'

            var sub = connection.GetSubscriber();
            //var subChannel = sub.Subscribe("__key*__:*");
            //var subChannel = sub.Subscribe("__keyevent@1__:*");
            var subChannel = sub.Subscribe("__keyevent@*__:*");
            int len = "__keyevent@".Length;
            subChannel.OnMessage(async (msg) =>
            {
                string s = msg.ToString().Substring(len), m = msg.Message.ToString();
                await _appHubContext.Clients.All.SendAsync("MessageRedis", s);
                //_logger.LogInformation($"REDIS_MONITORING -> {s}");
                //await _appHubContext.Clients.Group(HubsConstants.APP_HUB).ReceivePost(notification);
            });

            await Task.Delay(Timeout.Infinite, stoppingToken);
            _logger.LogDebug($"{ServiceName} is stopping.");
        }
    }
}
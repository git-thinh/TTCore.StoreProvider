using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using TTCore.StoreProvider.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;

namespace TTCore.StoreProvider.ServiceBackground
{
    public class NotificationBroker : BackgroundService
    {
        private readonly IHubContext<AppHub, IAppHub> _appHubContext;

        private readonly ILogger _logger;

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public NotificationBroker(
            IHubContext<AppHub, IAppHub> appHubContext,
            ILoggerFactory loggerFactory)
        {
            _appHubContext = appHubContext;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        private readonly string ServiceName = nameof(NotificationBroker);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation($"{ServiceName} background task is stopping."));

            ISubscriber sub = RedisConnection.Connection.GetSubscriber();

            // Subscribe to channels
            sub.Subscribe("PostAdded", async (channel, message) => {
                _logger.LogInformation($"PostAdded -> {message}");

                var notification = JsonSerializer
                    .Deserialize<PostChangedNotification>(message, jsonOptions);

                await _appHubContext.Clients
                    .Group(HubsConstants.APP_HUB)
                    .ReceivePost(notification);
            });

            sub.Subscribe("PostDeleted", async (channel, message) => {
                _logger.LogInformation($"PostDeleted -> {message}");

                var notification = JsonSerializer
                    .Deserialize<PostChangedNotification>(message, jsonOptions);

                await _appHubContext.Clients
                    .Group(HubsConstants.APP_HUB)
                    .DeletePost(notification);
            });

            await Task.Delay(Timeout.Infinite, stoppingToken);

            _logger.LogDebug($"{ServiceName} is stopping.");
        }
    }
}
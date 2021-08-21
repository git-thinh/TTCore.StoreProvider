using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TTCore.StoreProvider.Grpc;

namespace TTCore.StoreProvider.ServiceBackground
{
    public class GrpcService : BackgroundService
    {
        readonly ILogger _logger;
        readonly string ServiceName = nameof(GrpcService);
        public GrpcService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} background task is stopping."));

            var server = new Server()
            {
                Services = { Helloworld.Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort("localhost", 101010, ServerCredentials.Insecure) }
            };
            server.Start();

            await Task.Delay(Timeout.Infinite, stoppingToken);
            _logger.LogDebug($"{ServiceName} is stopping.");
        }
    }
}
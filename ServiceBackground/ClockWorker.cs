using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TTCore.StoreProvider.Hubs;
using TTCore.StoreProvider.Interfaces;

namespace TTCore.StoreProvider.ServiceBackground
{
    public class ClockWorker : BackgroundService
    {
        private readonly ILogger<ClockWorker> _logger;
        private readonly IHubContext<ClockHub, IClock> _clockHub;

        public ClockWorker(ILogger<ClockWorker> logger, IHubContext<ClockHub, IClock> clockHub)
        {
            _logger = logger;
            _clockHub = clockHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {Time}", DateTime.Now);
                //await _clockHub.Clients.All.SendAsync("ReceiveMessage", DateTime.Now.ToString());
                await _clockHub.Clients.All.ShowTime(DateTime.Now);
                await Task.Delay(1000);
            }
        }
    }
}
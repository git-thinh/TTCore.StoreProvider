using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Parsers.Rss;

namespace TTCore.StoreProvider.ServiceBackground
{
    public class FeedService : IHostedService, IDisposable
    {
        int executionCount = 0;
        readonly ILogger<TimedHostedService> _logger;
        Timer _timer;
        readonly TimeSpan timeOut = TimeSpan.FromHours(1);
        readonly string ServiceName = nameof(FeedService);

        public FeedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            DoWork(null);
            _logger.LogInformation($"{ServiceName} Service running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, timeOut);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            //_logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
            var url = "https://vnexpress.net/rss/tin-moi-nhat.rss";
            var rss = ParseRSSAsync(url).GetAwaiter().GetResult();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<RssSchema>> ParseRSSAsync(string feed)
        {
            IEnumerable<RssSchema> rss = null;

            using (var client = new HttpClient())
            {
                try
                {
                    feed = await client.GetStringAsync(feed);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            if (feed != null)
            {
                var parser = new RssParser();
                rss = parser.Parse(feed);
            }

            return rss;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

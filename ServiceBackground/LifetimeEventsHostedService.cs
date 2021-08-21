using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TTCore.StoreProvider.ServiceBackground
{
    internal class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger, 
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            try
            {
                ////var invoker = new Grpc.Core.CallInvoker( // .DefaultCallInvoker(channel);
                ////var client = new Greet.Greeter.GreeterClient(invoker);
                ////try
                ////{
                ////    var reply = client.SayHello(new Greet.HelloRequest() { Name = "123" });
                ////}
                ////catch (Exception exp)
                ////{
                ////    if (exp == null) return;
                ////}


                //////var channel = new Channel("localhost:5000", Grpc.Core.ChannelCredentials.Insecure);
                ////////////var client = new Greeter.GreeterClient(channel);
                ////////////var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });

                ////////////var httpHandler = new HttpClientHandler();
                ////////////httpHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                ////////////var httpClient = new HttpClient(httpHandler);
                ////////////var channel = GrpcChannel.ForAddress("http://localhost:42656", new GrpcChannelOptions
                ////////////{
                ////////////    HttpClient = httpClient
                ////////////});
                
                //////var client = new Greet.Greeter.GreeterClient(channel);
                //////var reply = client.SayHello(new Greet.HelloRequest() { Name = "123" });
            }
            catch (Exception ex)
            {
            }

        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            // Perform post-stopped activities here
        }
    }
}

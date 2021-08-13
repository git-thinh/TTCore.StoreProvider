using TTCore.StoreProvider.Data;
using TTCore.StoreProvider.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTCore.StoreProvider.Hubs;
using TTCore.StoreProvider.ServiceBackground;

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
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
            services.AddTransient<FactoryActivatedMiddleware>();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins("http://localhost:44317")
                       .AllowCredentials();
            }));

            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger.Api", Version = "v1" }); });
            services.AddRazorPages();

            services.AddSignalR();
            services.AddHostedService<ClockWorker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            #region WebSockets

            //var webSocketOptions = new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(120),
            //};
            //webSocketOptions.AllowedOrigins.Add("https://client.com");
            //webSocketOptions.AllowedOrigins.Add("https://www.client.com");
            app.UseWebSockets(new WebSocketOptions());
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            await WebSocketEcho(context, webSocket);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    await next();
                }
            });

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/ws-background")
            //    {
            //        if (context.WebSockets.IsWebSocketRequest)
            //        {
            //            using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
            //            {
            //                var socketFinishedTcs = new TaskCompletionSource<object>();
            //                BackgroundSocketProcessor.AddSocket(webSocket, socketFinishedTcs);
            //                await socketFinishedTcs.Task;
            //            }
            //        }
            //        else
            //        {
            //            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //        }
            //    }
            //    else
            //    {
            //        await next();
            //    }
            //});

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AIT.UI.Api v1"));

            //--------------------------------------------------------

            //--------------------------------------------------------

            app.UseMiddleware<RequestCultureMiddleware>();
            app.UseMiddleware<FactoryActivatedMiddleware>();

            //--------------------------------------------------------

            app.UseStaticFiles();
            //app.UseFileServer();
            //--------------------------------------------------------

            app.UseRouting();

            app.UseCors("CorsPolicy");
            //// Global cors policy
            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());


            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<ClockHub>("/hubs/clock");

                #region POST Stream Pipe

                //endpoints.MapGet("/", async context => { context.Response.Redirect("/swagger"); await context.Response.CompleteAsync(); });
                endpoints.MapPost("/Streams", async context =>
                {
                    var list = await GetListOfStringsFromStream(context.Request.Body);
                    foreach (var item in list)
                    {
                        await context.Response.WriteAsync(item.ToUpperInvariant());
                        await context.Response.WriteAsync(Environment.NewLine);
                    }
                });

                endpoints.MapPost("/BetterStreams", async context =>
                {
                    var list = await GetListOfStringsFromStreamMoreEfficient(context.Request.Body);
                    foreach (var item in list)
                    {
                        await context.Response.WriteAsync(item.ToUpperInvariant());
                        await context.Response.WriteAsync(Environment.NewLine);
                    }
                });

                endpoints.MapPost("/Pipes", async context =>
                {
                    var list = await GetListOfStringFromPipe(context.Request.BodyReader);
                    foreach (var item in list)
                    {
                        await context.Response.WriteAsync(item.ToUpperInvariant());
                        await context.Response.WriteAsync(Environment.NewLine);
                    }
                });

                #endregion


                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        #region WebSocket Echo

        private async Task WebSocketEcho(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        #endregion

        #region GetListOfStringsFromStream

        private async Task<List<string>> GetListOfStringsFromStream(Stream requestBody)
        {
            // Build up the request body in a string builder.
            StringBuilder builder = new StringBuilder();

            // Rent a shared buffer to write the request body into.
            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);

            while (true)
            {
                var bytesRemaining = await requestBody.ReadAsync(buffer, offset: 0, buffer.Length);
                if (bytesRemaining == 0)
                {
                    break;
                }

                // Append the encoded string into the string builder.
                var encodedString = Encoding.UTF8.GetString(buffer, 0, bytesRemaining);
                builder.Append(encodedString);
            }

            ArrayPool<byte>.Shared.Return(buffer);

            var entireRequestBody = builder.ToString();

            // Split on \n in the string.
            return new List<string>(entireRequestBody.Split("\n"));
        }

        #endregion

        #region GetListOfStringsFromStreamMoreEfficient

        private async Task<List<string>> GetListOfStringsFromStreamMoreEfficient(Stream requestBody)
        {
            StringBuilder builder = new StringBuilder();
            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
            List<string> results = new List<string>();

            while (true)
            {
                var bytesRemaining = await requestBody.ReadAsync(buffer, offset: 0, buffer.Length);

                if (bytesRemaining == 0)
                {
                    results.Add(builder.ToString());
                    break;
                }

                // Instead of adding the entire buffer into the StringBuilder
                // only add the remainder after the last \n in the array.
                var prevIndex = 0;
                int index;
                while (true)
                {
                    index = Array.IndexOf(buffer, (byte)'\n', prevIndex);
                    if (index == -1)
                    {
                        break;
                    }

                    var encodedString = Encoding.UTF8.GetString(buffer, prevIndex, index - prevIndex);

                    if (builder.Length > 0)
                    {
                        // If there was a remainder in the string buffer, include it in the next string.
                        results.Add(builder.Append(encodedString).ToString());
                        builder.Clear();
                    }
                    else
                    {
                        results.Add(encodedString);
                    }

                    // Skip past last \n
                    prevIndex = index + 1;
                }

                var remainingString = Encoding.UTF8.GetString(buffer, prevIndex, bytesRemaining - prevIndex);
                builder.Append(remainingString);
            }

            ArrayPool<byte>.Shared.Return(buffer);

            return results;
        }

        #endregion

        #region GetListOfStringFromPipe

        private async Task<List<string>> GetListOfStringFromPipe(PipeReader reader)
        {
            List<string> results = new List<string>();

            while (true)
            {
                ReadResult readResult = await reader.ReadAsync();
                var buffer = readResult.Buffer;

                SequencePosition? position = null;

                do
                {
                    // Look for a EOL in the buffer
                    position = buffer.PositionOf((byte)'\n');

                    if (position != null)
                    {
                        var readOnlySequence = buffer.Slice(0, position.Value);
                        AddStringToList(results, in readOnlySequence);

                        // Skip the line + the \n character (basically position)
                        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                    }
                }
                while (position != null);


                if (readResult.IsCompleted && buffer.Length > 0)
                {
                    AddStringToList(results, in buffer);
                }

                reader.AdvanceTo(buffer.Start, buffer.End);

                // At this point, buffer will be updated to point one byte after the last
                // \n character.
                if (readResult.IsCompleted)
                {
                    break;
                }
            }

            return results;
        }

        private static void AddStringToList(List<string> results, in ReadOnlySequence<byte> readOnlySequence)
        {
            // Separate method because Span/ReadOnlySpan cannot be used in async methods
            ReadOnlySpan<byte> span = readOnlySequence.IsSingleSegment ? readOnlySequence.First.Span : readOnlySequence.ToArray().AsSpan();
            results.Add(Encoding.UTF8.GetString(span));
        }

        #endregion
    }

    internal class BackgroundSocketProcessor
    {
        internal static void AddSocket(WebSocket socket, TaskCompletionSource<object> socketFinishedTcs)
        {
            //throw new NotImplementedException();
        }
    }
}

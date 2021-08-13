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
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using TTCore.StoreProvider.Extentions;

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
            services.AddDbContext<DbMemoryContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
            services.AddTransient<FactoryActivatedMiddleware>();

            services.AddCorsPolicyService();
            services.AddTransient<ValidateMimeMultipartContentFilter>();



            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger.Api", Version = "v1" }); });
            services.AddRazorPages();

            services.AddSignalRService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebsocketMiddleware();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AIT.UI.Api v1"));

            //--------------------------------------------------------

            app.UseMiddleware<RequestCultureMiddleware>();
            app.UseMiddleware<FactoryActivatedMiddleware>();

            //--------------------------------------------------------

            //app.UseFileServer();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "test")),
                RequestPath = "/test"
            });

            //--------------------------------------------------------

            app.UseRouting();

            app.UseCorsPolicyMiddleware();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSignalREndpointRoute();

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

}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace TTCore.StoreProvider.Middleware.Extentions
{
    public static class StaticFileExtention
    {
        public static void UseStaticFileMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseFileServer();
            //app.UseFileServer(enableDirectoryBrowsing: true);

            //var options = new DefaultFilesOptions();
            //options.DefaultFileNames.Clear();
            //options.DefaultFileNames.Add("mydefault.html");
            //app.UseDefaultFiles(options);

            //// Set up custom content types - associating file extension to MIME type
            //var provider = new FileExtensionContentTypeProvider();
            //// Add new mappings
            //provider.Mappings[".myapp"] = "application/x-msdownload";
            //provider.Mappings[".htm3"] = "text/html";
            //provider.Mappings[".image"] = "image/png";
            //// Replace an existing mapping
            //provider.Mappings[".rtf"] = "application/x-msdownload";
            //// Remove MP4 videos.
            //provider.Mappings.Remove(".mp4");

            const string cacheMaxAge = "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                //ServeUnknownFileTypes = true,
                //DefaultContentType = "image/png",

                //FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "images")),
                //RequestPath = "/MyImages",

                //ContentTypeProvider = provider,

                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cacheMaxAge}");
                }
            });

            foreach (var dir in Directory.GetDirectories(env.WebRootPath))
            {
                string folder = Path.GetFileName(dir);
                string dirTest = Path.Combine(env.WebRootPath, folder);
                app.UseDirectoryBrowser(new DirectoryBrowserOptions
                {
                    FileProvider = new PhysicalFileProvider(dirTest),
                    RequestPath = "/" + folder
                });
            }
        }
    }
}

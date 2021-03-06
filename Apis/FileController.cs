using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using TTCore.StoreProvider.Hubs;
using TTCore.StoreProvider.Middleware;
using TTCore.StoreProvider.Models;
using TTCore.StoreProvider.Services;

namespace TTCore.StoreProvider.Apis
{
    [ApiRouteBase]
    public class FileController : Controller
    {
        readonly IHubContext<ImageMessageHub> _hubContext;
        readonly IImageService _imageService;

        public FileController(IHubContext<ImageMessageHub> hubContext,
            IImageService imageService)
        {
            _hubContext = hubContext;
            _imageService = imageService;
        }

        [Route("upload")]
        [HttpPost]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public async Task<dynamic> UploadFiles(List<IFormFile> files)
        {
            if (files == null) files = new List<IFormFile>();
            var files2 = HttpContext.Request.Form.Files;
            if (files.Count == 0 && files2 != null && files2.Count > 0)
                foreach (var file in files2) files.Add(file);

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var buf = _imageService.ConvertWebP(file, 75);

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);

                            var imageMessage = new ImageMessage
                            {
                                ImageHeaders = "data:" + file.ContentType + ";base64,",
                                ImageBinary = memoryStream.ToArray()
                            };

                            await _hubContext.Clients.All.SendAsync("ImageMessage", imageMessage);
                        }
                    }
                }
            }

            return new { Ok = true, Count = files.Count };
        }
    }
}


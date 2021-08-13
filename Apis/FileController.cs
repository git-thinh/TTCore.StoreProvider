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

namespace TTCore.StoreProvider.Apis
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        readonly IHubContext<ImageMessageHub> _hubContext;

        public FileController(IHubContext<ImageMessageHub> hubContext)
        {
            _hubContext = hubContext;
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


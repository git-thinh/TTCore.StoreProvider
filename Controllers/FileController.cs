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

namespace Mascot.SharePoint.Service
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IHubContext<ImagesMessageHub> _hubContext;

        public FileController(IHubContext<ImagesMessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [Route("upload")]
        [HttpPost]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            var files2 = HttpContext.Request.Form.Files;

            if (ModelState.IsValid && files != null && files.Count > 0)
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

            return Redirect("/test/upload.html");
        }
    }
}


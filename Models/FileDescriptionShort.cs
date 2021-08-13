using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace TTCore.StoreProvider.Models
{
    public class FileDescriptionShort
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<IFormFile> File { get; set; }
    }
}
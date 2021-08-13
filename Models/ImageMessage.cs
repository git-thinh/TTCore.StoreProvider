using System;

namespace TTCore.StoreProvider.Models
{
    public class ImageMessage
    {
        public byte[] ImageBinary { get; set; }
        public string ImageHeaders { get; set; }
    }
}
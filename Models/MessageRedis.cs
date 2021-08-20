using System;

namespace TTCore.StoreProvider.Models
{
    public class MessageRedis
    {
        public int TimeCreated { get; set; }
        public int DateCreated { get; set; }
        public string Command { get; set; }
        public string Key { get; set; }

        public MessageRedis() {
            var dt = DateTime.Now;
            TimeCreated = int.Parse(dt.ToString("HHmmss"));
            DateCreated = int.Parse(dt.ToString("yyMMdd"));
        }
    }
}

using System;

namespace TTCore.StoreProvider.Models
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public int GroupId { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }
    }
}

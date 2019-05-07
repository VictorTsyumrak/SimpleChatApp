using System;

namespace SimpleChatApp.Domain
{
    public class Message
    {
        public Guid ConnectionId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public long Timestamp { get; set; }
    }
}
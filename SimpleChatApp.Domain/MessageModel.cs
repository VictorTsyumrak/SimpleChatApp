using Newtonsoft.Json;

namespace SimpleChatApp.Domain
{
    public class MessageModel
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; } 
    }
}
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleChatApp.Abstractions.Connectivity;
using SimpleChatApp.Domain.Extensions;

namespace SimpleChatApp.Domain
{
    public class User
    {
        private readonly IConnection _connection;
        private Room _room;
        public Guid ConnectionId { get; set; }

        public User(IConnection connection)
        {
            _connection = connection;
        }

        public void Join(Room room)
        {
            _room?.Leave(ConnectionId);
            _room = room;
        }

        public void Leave()
        {
            _room?.Leave(ConnectionId);
            _room = null;
        }

        public Task AcceptAsync(Message message)
        {
            return _connection?.SendJsonAsync(message) ?? Task.CompletedTask;
        }

        public Task SendAsync(byte[] bytes)
        {
            var message = Encoding.UTF8.GetString(bytes);
            var messageModel = JsonConvert.DeserializeObject<MessageModel>(message);
            
            return _room?.SendToAllAsync(new Message
            {
                ConnectionId = ConnectionId,
                Content = messageModel.Content,
                Timestamp = DateTime.UtcNow.ToFileTime(),
                AuthorName = string.IsNullOrWhiteSpace(messageModel.Name) ? "No Name" : messageModel.Name
            });
        }
    }
}
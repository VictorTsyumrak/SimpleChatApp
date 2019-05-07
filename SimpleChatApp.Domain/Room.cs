using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleChatApp.Domain.Options;

namespace SimpleChatApp.Domain
{
    public class Room
    {
        private readonly ConcurrentDictionary<Guid, User> _users;
        private readonly ConcurrentQueue<Message> _messages;
        private readonly int _maxMessagesCount;
        public Room(RoomOptions config, bool isDefault)
        {
            _users = new ConcurrentDictionary<Guid, User>();
            _messages = new ConcurrentQueue<Message>();
            Capacity = config.Capacity;
            IsDefault = isDefault;
            _maxMessagesCount = config.MaxMessagesCount;
        }

        public bool IsDefault { get; }
        public int Capacity { get; }

        public bool IsAvailable => _users.Count < Capacity;
        
        public void Join(User user)
        {
            if (!IsAvailable) return;
            
            _users.TryAdd(user.ConnectionId, user);
            user.Join(this);
        }

        public void Leave(Guid connectionId)
        {
            _users.TryRemove(connectionId, out _);
        }

        public Task SendToAllAsync(Message message)
        {
            AddMessage(message);
            var messageSendTasks = _users.Select(kv => kv.Value.AcceptAsync(message)).ToArray();
            return Task.WhenAll(messageSendTasks);
        }

        public IReadOnlyCollection<Message> GetMessages()
        {
            return _messages.Take(_maxMessagesCount).ToList();
        }

        public User GetUser(Guid connectionId) => _users.TryGetValue(connectionId, out var user) ? user : null;

        private void AddMessage(Message message)
        {
            _messages.Enqueue(message);
            while (_messages.Count > _maxMessagesCount)
            {
                _messages.TryDequeue(out _);
            }
        }
    }
}
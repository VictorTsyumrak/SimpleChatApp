using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using SimpleChatApp.Abstractions.Connectivity;
using SimpleChatApp.Domain.Options;

namespace SimpleChatApp.Domain
{
    public class ChatHub
    {
        private readonly IConnectionManager _connectionManager;
        private ConcurrentDictionary<int, Room> _rooms;

        public ChatHub(IConnectionManager connectionManager, IOptions<ChatOptions> chatConfig)
        {
            _connectionManager = connectionManager;
            ConfigureRooms(chatConfig.Value);
        }

        public void Join(Guid connectionId, int roomId)
        {
            var user = new User(_connectionManager.GetById(connectionId))
            {
               ConnectionId = connectionId
            };
            
            if (_rooms.TryGetValue(roomId, out var room))
            {
                room?.Join(user);
                return;
            };
            
            JoinDefaultRoom(user);
        }

        public User GetUser(Guid connectionId) =>
            _rooms.Select(r => r.Value?.GetUser(connectionId)).FirstOrDefault(u => u != null);

        public void Leave(Guid connectionId)
        {
            var user = GetUser(connectionId);
            user?.Leave();
        }

        public IReadOnlyCollection<Message> GetLastMessages(int groupId)
        {
            _rooms.TryGetValue(groupId, out var room);
            return room?.GetMessages();
        }

        public IReadOnlyDictionary<int, Room> GetRooms() => _rooms;

        private void JoinDefaultRoom(User user)
        {
            var room = _rooms.Select(r => r.Value).FirstOrDefault(r => r.IsDefault);
            room?.Join(user);
        }

        private void ConfigureRooms(ChatOptions config)
        {
            _rooms = new ConcurrentDictionary<int, Room>
            {
                [0] = new Room(config.Room, true)
            };
            for (int i = 1; i < config.NumberOfRooms; i++)
            {
                _rooms.TryAdd(i, new Room(config.Room, false));
            }
        }
    }
}
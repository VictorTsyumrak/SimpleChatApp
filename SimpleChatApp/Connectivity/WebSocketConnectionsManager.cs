using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using SimpleChatApp.Abstractions.Connectivity;

namespace SimpleChatApp.Connectivity
{
    public class WebSocketConnectionsManager : IConnectionManager
    {
        private readonly ConcurrentDictionary<Guid, IConnection> _connections;

        public WebSocketConnectionsManager()
        {
            _connections = new ConcurrentDictionary<Guid, IConnection>();
        }
        
        public void Connect(IConnection connection)
        {
            var guid = Guid.NewGuid();
            _connections.TryAdd(guid, connection);
        }

        public async Task DisconnectAsync(Guid connectionId)
        {
            _connections.TryRemove(connectionId, out var connection);
            if (connection != null)
            {
                await connection.CloseAsync();
            }
        }

        public IConnection GetById(Guid connectionId)
        {
            _connections.TryGetValue(connectionId, out var socket);
            return socket;
        }

        public Guid GetId(IConnection connection)
        {
            return _connections.FirstOrDefault(e => e.Value == connection).Key;
        }
    }
}
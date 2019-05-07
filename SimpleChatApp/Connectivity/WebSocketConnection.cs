using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleChatApp.Abstractions.Connectivity;

namespace SimpleChatApp.Connectivity
{
    public class WebSocketConnection : IConnection
    {
        private readonly WebSocket _socket;

        public WebSocketConnection(WebSocket socket)
        {
            _socket = socket;
        }
        
        public Task SendAsync(byte[] content)
        {
            return _socket.SendAsync(new ArraySegment<byte>(content), WebSocketMessageType.Text, true,
                    CancellationToken.None);
        }

        public Task CloseAsync()
        {
            return _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }

        public override bool Equals(object obj)
        {
            return _socket.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _socket.GetHashCode();
        }
    }
}
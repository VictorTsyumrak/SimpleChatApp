using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SimpleChatApp.Abstractions.Connectivity;
using SimpleChatApp.Connectivity;

namespace SimpleChatApp.Middlewares
{
    public class WebSocketHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHandler _handler;
        private readonly IOptions<WebSocketOptions> _options;
        private readonly IConnectionManager _manager;

        public WebSocketHandlerMiddleware(RequestDelegate next, IHandler handler, IOptions<WebSocketOptions> options, IConnectionManager manager)
        {
            _next = next;
            _handler = handler;
            _options = options;
            _manager = manager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next(context);
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var connection = new WebSocketConnection(socket);
            _manager.Connect(connection);
            
            var connectionId = _manager.GetId(connection);
            _handler.Connect(connectionId, GetGroupId(context));

            await Receive(socket, async (result, data) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await _handler.HandleAsync(connectionId, data);
                    return;
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _handler.Disconnect(connectionId);
                    await _manager.DisconnectAsync(connectionId);
                }
            });
        }
        
        private async Task Receive(WebSocket socket, Func<WebSocketReceiveResult, byte[], Task> handleMessage)
        {
            var buffer = new byte[_options.Value.ReceiveBufferSize];

            while(socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;
                var response = new List<byte>();
                do
                {
                    result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    response.AddRange(buffer.Take(result.Count));

                } while (!result.EndOfMessage);

                await handleMessage(result, response.ToArray());                
            }
        }

        private int GetGroupId(HttpContext context)
        {
            var value = context.Request.Query["groupId"];
            int.TryParse(value, out var groupId);
            return groupId;
        }
    }
}
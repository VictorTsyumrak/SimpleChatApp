using System;
using System.Threading.Tasks;
using SimpleChatApp.Abstractions.Connectivity;
using SimpleChatApp.Domain;

namespace SimpleChatApp.Connectivity
{
    public class ChatHandler : IHandler
    {
        private readonly ChatHub _chatHub;

        public ChatHandler(ChatHub chatHub)
        {
            _chatHub = chatHub;
        }

        public void Connect(Guid connectionId, int groupId) => _chatHub.Join(connectionId, groupId);

        public void Disconnect(Guid connectionId) => _chatHub.Leave(connectionId);

        public Task HandleAsync(Guid connectionId, byte[] content)
        {
            var user = _chatHub.GetUser(connectionId);
            return user?.SendAsync(content) ?? Task.CompletedTask;
        }
    }
}
using System;
using System.Threading.Tasks;

namespace SimpleChatApp.Abstractions.Connectivity
{
    public interface IHandler
    {
        void Connect(Guid connectionId, int groupId);
        void Disconnect(Guid connectionId);
        Task HandleAsync(Guid connectionId, byte[] content);
    }
}
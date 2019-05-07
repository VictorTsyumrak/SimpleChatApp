using System;
using System.Threading.Tasks;

namespace SimpleChatApp.Abstractions.Connectivity
{
    public interface IConnectionManager
    {
        void Connect(IConnection connection);
        Task DisconnectAsync(Guid connectionId);
        IConnection GetById(Guid connectionId);
        Guid GetId(IConnection connection);
    }
}
using System;
using System.Threading.Tasks;

namespace SimpleChatApp.Abstractions.Connectivity
{
    public interface IConnection
    {
        Task SendAsync(byte[] content);
        Task CloseAsync();
    }
}
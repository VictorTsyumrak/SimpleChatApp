using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleChatApp.Abstractions.Connectivity;

namespace SimpleChatApp.Domain.Extensions
{
    public static class ConnectionExtensions
    {
        public static Task SendJsonAsync<T>(this IConnection connection, T message)
        {
            return connection?.SendAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))) ?? Task.CompletedTask;
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApp.Abstractions.Connectivity;
using SimpleChatApp.Connectivity;
using SimpleChatApp.Domain;

namespace SimpleChatApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChatServices(this IServiceCollection collection)
        {
            collection.AddTransient<IHandler, ChatHandler>();
            collection.AddSingleton<ChatHub>();
            
            return collection;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using SimpleChatApp.Domain;

namespace SimpleChatApp.ViewComponents
{
    public class ChatViewComponent : ViewComponent
    {
        private readonly ChatHub _chatHub;

        public ChatViewComponent(ChatHub chatHub)
        {
            _chatHub = chatHub;
        }

        public IViewComponentResult Invoke(int roomId)
        {
            return View(_chatHub.GetLastMessages(roomId));
        }
    }
}
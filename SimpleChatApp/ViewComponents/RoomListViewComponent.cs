using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleChatApp.Domain;
using SimpleChatApp.Models;

namespace SimpleChatApp.ViewComponents
{
    public class RoomListViewComponent : ViewComponent
    {
        private readonly ChatHub _chatHub;

        public RoomListViewComponent(ChatHub chatHub)
        {
            _chatHub = chatHub;
        }

        public IViewComponentResult Invoke()
        {
            var rooms = _chatHub.GetRooms().Select(r => new RoomViewModel
            {
                IsDefault = r.Value.IsDefault,
                IsAvailable = r.Value.IsAvailable,
                RoomId = r.Key
            }).ToImmutableList();
            
            return View(rooms);
        }
    }
}
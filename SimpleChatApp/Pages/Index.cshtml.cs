using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleChatApp.Domain;

namespace SimpleChatApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int GroupId { get; private set; }

        public void OnGet(int groupId)
        {
            GroupId = groupId;
        }
    }
}
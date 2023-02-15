using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.OrganizerEvent
{
    public class IndexModel : PageModel
    {
        private readonly EventService _eventService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public IndexModel(EventService eventService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _eventService = eventService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public List<Event> EventList { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            EventList = _eventService.GetAllByUserId(userId);

            return Page();
        }
    }
}

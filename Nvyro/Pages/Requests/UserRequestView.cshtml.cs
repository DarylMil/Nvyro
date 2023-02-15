using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Services;
using Nvyro.Models;
using Microsoft.AspNetCore.Identity;

namespace Nvyro.Pages.Requests
{
    public class UserRequestViewModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RequestService _RequestService;
        private readonly EventService _EventService;
        public UserRequestViewModel(RequestService requestService, UserManager<ApplicationUser> userManager, EventService eventService)
        {
            _userManager = userManager;
            _RequestService = requestService;
            _EventService = eventService;
        }
        public List<Request> AllRequests { get; set; } = new();
        [BindProperty]
        public Event currentEvent { get; set; }

        public async Task OnGetAsync()
        {
            var existingUser = await _userManager.GetUserAsync(User);
            AllRequests = _RequestService.GetAll(existingUser.Id);
            foreach(var request in AllRequests)
            {
                Event? Event = _EventService.GetEventById(request.CopyEventId);
                currentEvent = Event;
            }
        }
    }
}

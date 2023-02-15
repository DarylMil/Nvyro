using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.Requests
{
    public class OrganizerViewAllRequestsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RequestService _RequestService;
        private readonly EventService _EventService;

        [BindProperty]
        public Event currentEvent { get; set; }

        public int NotCollected { get; set; } = 0;
        public int Collected { get; set; } = 0;

        public OrganizerViewAllRequestsModel(UserManager<ApplicationUser> userManager, RequestService requestService, EventService eventService)
        {
            _userManager = userManager;
            _RequestService = requestService;
            _EventService = eventService;
        }
        public List<Request> AllRequestsForEvent { get; set; } = new();

        public async void OnGet(int Id)
        {
            Event? Event = _EventService.GetEventById(Id);
            Count(Id);
            currentEvent = Event;
            AllRequestsForEvent = _RequestService.GetAllEventsRequests(Id);
        }

        public void Count(int Id)
        {
            foreach(var i in _RequestService.GetAllCollectedRequests(Id))
            {
                Collected++;
            }
            foreach (var i in _RequestService.GetAllUnCollectedRequests(Id))
            {
                NotCollected++;
            }
        }
    }
}

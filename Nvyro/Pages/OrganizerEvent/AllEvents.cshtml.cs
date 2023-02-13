using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.OrganizerEvent
{
    public class AllEventsModel : PageModel
    {
        private readonly EventService _eventService;

        public AllEventsModel(EventService eventService)
        {
            _eventService = eventService;
        }

        public List<Event> EventList { get; set; } = new();

        public void OnGet()
        {
            EventList = _eventService.GetAll();
        }
    }
}


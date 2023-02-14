using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;


namespace Nvyro.Pages
{
    public class IndexModel : PageModel
    {

        private readonly EventService _eventService;

        public IndexModel(EventService eventService)
        {
            _eventService = eventService;
            _eventService = eventService;
        }

        public List<Event> EventList { get; set; } = new();

        public void OnGet()
        {
            EventList = _eventService.GetAll();
        }
    }
}
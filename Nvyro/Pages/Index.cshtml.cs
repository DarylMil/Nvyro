using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Services;
using Nvyro.Models;
using System.ComponentModel.Design;


namespace Nvyro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly EventService _eventService;

        public IndexModel(ILogger<IndexModel> logger, EventService eventService)
        {

            _logger = logger;
            _eventService = eventService;
        }

        public List<Event> Allevents { get; set; } = new();

        public void OnGet()
        {
            Allevents = _eventService.GetAll();
        }

    
    }
}
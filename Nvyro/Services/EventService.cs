using Nvyro.Data;
using Nvyro.Models;

namespace Nvyro.Services
{
    public class EventService
    {
        private readonly RequestService _requestService;
        private readonly MyDbContext? _context;

        public EventService(MyDbContext context, RequestService requestService)
        {
            _context = context;
            _requestService = requestService;
        }

        public List<Event> GetAllEvents()
        {
            return _context.Events.ToList();
        }

        public Event? GetEventByTitle(string EventTitle)
        {
            Event foundevent = _context.Events.FirstOrDefault(x => x.EventTitle.Equals(EventTitle));
            return foundevent;
        }
            
    }
}

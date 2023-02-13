using Nvyro.Data;
using Nvyro.Models;

namespace Nvyro.Services
{
    public class EventService
    {
        private readonly RequestService _requestService;
        private readonly MyDbContext? _context;


        private readonly MyDbContext _context;

        public EventService(MyDbContext context)
        {
            _context = context;
            _requestService = requestService;
        }

        public List<Event> GetAll()
        {
            return _context.Events.OrderBy(x => x.Id).ToList();
        }

        public Event? GetEventById(int id)
        {
            Event? events = _context.Events.FirstOrDefault(x => x.Id.Equals(id));
            return events;
        }

        public void AddEvent(Event events)
        {
            _context.Events.Add(events);
            _context.SaveChanges();
        }

        public void UpdateEvent(Event events)
        {
            _context.Events.Update(events);
            _context.SaveChanges();
        }

    }
}

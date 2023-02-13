using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nvyro.Data;
using Nvyro.Models;

namespace Nvyro.Services
{
    public class RequestService
    {
        private readonly MyDbContext _context;

        public RequestService(MyDbContext context)
        {
            _context = context;
        }

        public void AddRequest(Request request, ApplicationUser applicationUser, Event currentEvent)
        {
            request.Event = currentEvent;
            request.Applicationuser = applicationUser;
            _context.Requests.Add(request);
            _context.SaveChanges();
        }

        public void AddRequestImages(Request request, Request_Images request_Images)
        {
            request_Images.Request = request;
            _context.Request_Images.Add(request_Images);
            _context.SaveChanges();
        }

        public List<Request> GetAll(string userId)
        {
            return _context.Requests.Where(r => r.Applicationuser.Id == userId).ToList();
        }
        public Request? GetRequestById(int Id)
        {
            Request? request = _context.Requests.FirstOrDefault(x => x.Id.Equals(Id));
            return request;
        }

        public Request? FindRequestifExist(string PostalCodeandUnit, string userId)
        {
            Request? request = GetAll(userId).FirstOrDefault(x => x.PostalCodeAndUnit.Equals(PostalCodeandUnit));
            return request;
        }

        public void UpdateRequest(Request request)
        {
            _context.Requests.Update(request);
            _context.SaveChanges();
        }
        public void DeleteRequest(Request request)
        {
            _context.Requests.Remove(request);
            _context.SaveChanges();
        }

/*        public List<Event> GetEventsWithUserRequest(string userId)
        {
            List<string> UserRequestsEventTitle = new List<string>();
            List<Event> EventsUserRequested = new List<Event>();
            foreach (var i in GetAll(userId))
            {
                UserRequestsEventTitle.Add(i.);
            }
            foreach (var i in UserRequestsEventTitle)
            {
                EventsUserRequested.Add((Event)_context.Events.Where(x => x.EventTitle == i));
            }
            return EventsUserRequested;
        }*/
    }
}

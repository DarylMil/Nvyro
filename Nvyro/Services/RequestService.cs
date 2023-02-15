using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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

        public List<Request> GetAllEventsRequests(int EventId)
        {
            return _context.Requests.Where(r => r.CopyEventId == EventId).ToList();
        }

        public List<Request> GetAllCollectedRequests(int EventId)
        {
            return _context.Requests
                .Where(r => r.CopyEventId == EventId)
                .Where(r => r.isCollected == true).ToList();
        }
        public List<Request> GetAllUnCollectedRequests(int EventId)
        {
            return _context.Requests
                .Where(r => r.CopyEventId == EventId)
                .Where(r => r.isCollected == false).ToList();
        }

        public Request? GetRequestById(int Id)
        {
            Request? request = _context.Requests.FirstOrDefault(x => x.Id.Equals(Id));
            return request;
        }

        public List<Request_Images> GetImagesById(int Id)
        {
            return _context.Request_Images.Where(r => r.Request.Id == Id).ToList();
        }

        public bool FindRequestifExist(string postalCode, string unitNumber, string userId, int EventId)
        {
            Console.WriteLine($"postalCode: {postalCode}");
            Console.WriteLine($"unitNumber: {unitNumber}");
            Console.WriteLine($"userId: {userId}");
            Console.WriteLine($"EventId: {EventId}");

            var request = _context.Requests
                .Include(r => r.Applicationuser)
                .Include(r => r.Event)
                .Where(r => r.PostalCode == postalCode)
                .Where(r => r.UnitNumber == unitNumber)
                .Where(r => r.Applicationuser.Id == userId)
                .Where(r => r.Event.Id == EventId)
                .FirstOrDefault();

            var isExist = request is not null;

            Console.WriteLine($"Is exist: {isExist}");
            
            return isExist;
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

        public void DeleteImages(int Id)
        {
            foreach (var i in GetImagesById(Id))
            {
                _context.Request_Images.Remove(i);
                _context.SaveChanges();
            }
        }
    }
}

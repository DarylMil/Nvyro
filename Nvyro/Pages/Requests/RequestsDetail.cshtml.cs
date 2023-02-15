using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Services;
using Nvyro.Models;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Nvyro.Pages.Requests
{
    public class RequestsDetailModel : PageModel
    {
        private readonly EventService _EventService;
        private readonly IWebHostEnvironment _environment;
        private readonly RequestService _RequestService;
        private readonly INotyfService _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public Request Request { get; set; }
        [BindProperty]
        public List<IFormFile> Upload { get; set; }

        [BindProperty]
        public Event currentEvent { get; set; }

        public List<Request_Images> Request_Images { get; set; } = new();

        public RequestsDetailModel(RequestService requestService, IWebHostEnvironment environment, EventService eventService, INotyfService ToastNotification , UserManager<ApplicationUser> userManager)
        {
            _RequestService = requestService;
            _environment = environment;
            _EventService = eventService;
            _toastNotification = ToastNotification;
            _userManager = userManager;
        }
        public void OnGet(int Id , int EventId)
        {
            Request = _RequestService.GetRequestById(Id);
            Event? Event = _EventService.GetEventById(EventId);
            currentEvent = Event;
            Request_Images = _RequestService.GetImagesById(Id);
        }

        public async Task OnPost()
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.GetUserAsync(User);
                var requestExists =  _RequestService.FindRequestifExist(Request.PostalCode, Request.UnitNumber, existingUser.Id, currentEvent.Id);
                if (!requestExists) 
                {
                    _RequestService.UpdateRequest(Request);
                    _toastNotification.Success("Your request has been successfully updated.");
                    Redirect("/Index");
                }
                else
                {
                    _toastNotification.Error("Seems like you already made a request to this event using this address. Try using a different address.");
                    Redirect("/Index");
                }
            }
            Redirect("/Index");
        }

        public async Task<IActionResult> OnPostSaveImages()
        {
            _RequestService.DeleteImages(Request.Id);
            if (Upload.Count < 4)
            {
                foreach (var i in Upload)
                {
                    if (i.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads/request";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(i.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await i.CopyToAsync(fileStream);

                    var request_images = new Request_Images
                    {
                        ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile)
                    };
                    Request requestforimg = _RequestService.GetRequestById(Request.Id);
                    _RequestService.AddRequestImages(requestforimg, request_images);
                }
                Request requestforimg1 = _RequestService.GetRequestById(Request.Id);
                _toastNotification.Success($"Request Id {requestforimg1.Id} has been successfully updated.");
            }
            else { _toastNotification.Error("Only 3 images allowed per request"); }
            return Redirect("/Requests/UserRequestView");
        }
        public IActionResult OnPostDelete()
        {
            if (ModelState.IsValid)
            {
                _RequestService.DeleteImages(Request.Id);
                _RequestService.DeleteRequest(Request);
                return Redirect("/Requests/UserRequestView");
            }
            return Page();
        }

        public bool ReqCheck(string StartPostalCode, string EndPostalCode, string PostalCode)
        {
            int startPostalCode = int.Parse(StartPostalCode);
            int endPostalCode = int.Parse(EndPostalCode);
            int newPostalCode = int.Parse(PostalCode);

            if (newPostalCode >= startPostalCode && newPostalCode <= endPostalCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

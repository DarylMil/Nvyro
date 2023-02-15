using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Services;
using Nvyro.Models;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;
using Nvyro.Models.DTO;

namespace Nvyro.Pages.Requests
{
    public class Request_formModel : PageModel
    {
        private readonly EventService _EventService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RequestService _RequestService;
        private IWebHostEnvironment _environment;
        private readonly INotyfService _toastNotification;

        public Request_formModel(RequestService requestService ,INotyfService ToastNotification, UserManager<ApplicationUser> userManager, EventService eventService, IWebHostEnvironment environment)
        {
            _RequestService = requestService;
            _userManager = userManager;
            _EventService = eventService;
            _environment = environment;
            _toastNotification = ToastNotification;
        }
        [BindProperty]
        public List<IFormFile> Upload { get; set; }

        [BindProperty]
        public Request newRequest { get; set; } = new();
        [BindProperty]
        public Event currentEvent { get; set; }

        public async Task OnPost()
        {
            var existingUser = await _userManager.GetUserAsync(User);
            var EventId = currentEvent.Id;
            Event? Event = _EventService.GetEventById(EventId);
            currentEvent = Event;

            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Count < 4)
                    {
                        if(newRequest.isUsingUserAddressString == "false")
                        {
                            newRequest.isUsingUserAddress = false;
                            _RequestService.AddRequest(newRequest, existingUser, Event);
                        }
                        if(newRequest.isUsingUserAddressString == "true")
                        {
                            newRequest.isUsingUserAddress = true;
                            _RequestService.AddRequest(newRequest, existingUser, Event);
                        }
                        foreach (var i in Upload)
                        {
                            if (i.Length > 2 * 1024 * 1024)
                            {
                                ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
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
                            _RequestService.AddRequestImages(newRequest, request_images);
                        }
                        _toastNotification.Success($"Your request with postal code {newRequest.PostalCode} has been successfully added.");
                        Redirect("/Index");
                    }
                    else { _toastNotification.Error("Only 3 images allowed per request"); }
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                    }
                }
            }
            Page();
        }
        public async Task OnGetAsync(int EventId)
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser == null)
            {
                Redirect("/Requests/UserRequestView");
            }
            else
            {
                newRequest.PostalCode = existingUser.PostalCode;
                newRequest.BlockNumber = existingUser.BlockNumber;
                newRequest.UnitNumber = existingUser.UnitNumber;
                newRequest.RoadName = existingUser.RoadName;

                Event? Event = _EventService.GetEventById(EventId);
                currentEvent = Event;
            }            
        }
    }
}

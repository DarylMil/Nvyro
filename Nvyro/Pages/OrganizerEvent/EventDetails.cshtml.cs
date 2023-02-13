using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.OrganizerEvent
{
    public class EventDetailsModel : PageModel
    {
        private readonly EventService _eventService;
        private IWebHostEnvironment _environment;
        private readonly INotyfService _toastNotification;

        public EventDetailsModel(EventService eventService, INotyfService toastNotification, IWebHostEnvironment environment)
        {
            _eventService = eventService;
            _toastNotification = toastNotification;
            _environment = environment;
        }

        [BindProperty]

        public Event myEvent { get; set; } 
       

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public IActionResult OnGet(int id)
        {
            Event? events = _eventService.GetEventById(id);
            if (events != null)
            {
                myEvent = events;
                return Page();
            }
            else
            {
                _toastNotification.Error("Event not found");
                return Redirect("/OrganizerEvent/Index");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "assets/EventImage";
                    if (myEvent.ImageURL != null)
                    {
                        var oldImageFile = Path.GetFileName(myEvent.ImageURL);
                        var oldImagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    myEvent.ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }

                _eventService.UpdateEvent(myEvent);
                _toastNotification.Success("Event updated");
            }
            return Redirect("/OrganizerEvent/Index");
        }
    }
}

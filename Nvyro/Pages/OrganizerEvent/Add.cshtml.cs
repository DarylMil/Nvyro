using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.OrganizerEvent
{
    public class AddModel : PageModel
    {
        private readonly EventService _eventService;
        private readonly INotyfService _toastNotification;
        private IWebHostEnvironment _environment;

        public AddModel(EventService eventService, INotyfService toastNotification, IWebHostEnvironment environment)
        {
            _eventService = eventService;
            _toastNotification = toastNotification;
            _environment = environment;
        }

        [BindProperty]
        public Event MyEvent { get; set; } = new();

        [BindProperty]
        public IFormFile? Upload { get; set; }


        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
              
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB");
                        return Page();
                    }

                    var uploadsFolder = "assets/EventImage";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    MyEvent.ImageURL = String.Format("/{0}/{1}", uploadsFolder, imageFile);
                }

                _eventService.AddEvent(MyEvent);
                _toastNotification.Success("Event is created");
                return Redirect("/OrganizerEvent/Index");
            }
            return Page();
        }
    }
}

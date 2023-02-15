using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.Requests
{
    public class OrganizerViewRequestModel : PageModel
    {
        private readonly RequestService _RequestService;
        private readonly INotyfService _toastNotification;
        public OrganizerViewRequestModel(RequestService requestService, INotyfService ToastNotification)
        {
            _RequestService = requestService;
            _toastNotification = ToastNotification;
        }
        public List<Request_Images> Request_Images { get; set; } = new();

        [BindProperty]
        public Request Request { get; set; }

        public void OnGet(int Id)
        {
            Request = _RequestService.GetRequestById(Id);
            Request_Images = _RequestService.GetImagesById(Id);
        }

        public async Task OnPost()
        {
            if (ModelState.IsValid)
            {
                if(Request.isCollectedString == "true")
                {
                    Request.isCollected = true;
                    _RequestService.UpdateRequest(Request);
                }
                if(Request.isCollectedString == "false")
                {
                    Request.isCollected = false;
                    _RequestService.UpdateRequest(Request);
                }
                _toastNotification.Success($"Request {Request.Id}'s status has been successfully changed.");
                Redirect("/Index");
            }
            Redirect("/Index");
        }
    }
}

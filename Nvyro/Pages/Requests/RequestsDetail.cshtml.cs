using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Services;
using Nvyro.Models;
using System.Net;
using Microsoft.AspNetCore.Hosting;

namespace Nvyro.Pages.Requests
{
    public class RequestsDetailModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly RequestService _RequestService;
        [BindProperty]
        public Request MyRequest { get; set; }

        public RequestsDetailModel(RequestService requestService, IWebHostEnvironment environment)
        {
            _RequestService = requestService;
            _environment = environment;
        }
        public void OnGet(int Id)
        {
            Request? request = _RequestService.GetRequestById(Id);
            MyRequest = request;
            var uploadsFolder = "uploads";
            /*if (MyRequest.ImageURL != null)
            {
                var oldImageFile = Path.GetFileName(MyRequest.ImageURL);
                var oldImagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }*/
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
            _RequestService.UpdateRequest(MyRequest);
            return Redirect("/Requests/UserRequestView");
            }
            
            
            return Page();

        }
        public IActionResult OnPostDelete()
        {
            if (ModelState.IsValid)
            {
                _RequestService.DeleteRequest(MyRequest);
                return Redirect("/Requests/UserRequestView");
            }
            return Page();
        }

    }
}

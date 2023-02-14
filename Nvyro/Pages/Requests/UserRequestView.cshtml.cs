using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Services;
using Nvyro.Models;
using Microsoft.AspNetCore.Identity;

namespace Nvyro.Pages.Requests
{
    public class UserRequestViewModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RequestService _RequestService;
        public UserRequestViewModel(RequestService requestService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _RequestService = requestService;
        }
        public List<Request> AllRequests { get; set; } = new();
        /*        public List<Event> UsersEvents { get; set; } = new();*/

        public async Task OnGetAsync()
        {
            var existingUser = await _userManager.GetUserAsync(User);
            AllRequests = _RequestService.GetAll(existingUser.Id);
            /*            UsersEvents = _RequestService.GetEventsWithUserRequest(existingUser.Id);*/
        }
    }
}

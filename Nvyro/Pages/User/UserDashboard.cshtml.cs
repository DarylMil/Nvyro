using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Models.DTO;
using Nvyro.Services;

namespace Nvyro.Pages.User
{
    [Authorize]
    public class UserDashboardModel : PageModel
    {
        [BindProperty]
        public UpdateUserModel AppUser { get; set; } = new UpdateUserModel();

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserAuthenticationService _userAuthService;
        private readonly INotyfService _toastNotification;

        public UserDashboardModel(UserManager<ApplicationUser> userManager, 
            UserAuthenticationService userAuthService, INotyfService toastNotification)
        {
            _userAuthService = userAuthService;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            
            var existingUser = await _userManager.GetUserAsync(User);
            var allRoles = await _userManager.GetRolesAsync(existingUser);

            Console.WriteLine(existingUser.Email);

            AppUser.Email = existingUser.Email;
            AppUser.PostalCode = existingUser.PostalCode;
            AppUser.BlockNumber = existingUser.BlockNumber;
            AppUser.UnitNumber = existingUser.UnitNumber;
            AppUser.RoadName = existingUser.RoadName;
            AppUser.PhoneNumber = existingUser.PhoneNumber;
            AppUser.IsDisabled = existingUser.IsDisabled;
            AppUser.ProfilePicURL = existingUser.ProfilePicURL;
            AppUser.FullName = existingUser.FullName;
            AppUser.RecycleCategories = existingUser.RecycleCategories;
            AppUser.Points = existingUser.Points;

            for (var i=0; i < allRoles.Count; i++)
            {
                AppUser.Roles += allRoles[i];
                if(i != allRoles.Count-1)
                {
                    AppUser.Roles += " | ";
                }
            }
        }
        public async Task<IActionResult> OnPostAsync() {
            
            var status = await _userAuthService.UpdateUserAsync(User,AppUser);
            if (status.StatusCode == 0)
            {
                _toastNotification.Error(status.Message);
                return Page();
            }
            else
            {
                _toastNotification.Success(status.Message);
                return Page();
            }
        }
    }
}

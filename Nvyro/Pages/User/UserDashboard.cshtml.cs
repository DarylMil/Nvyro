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
        private readonly INotyfService _toastNotification;

        private IWebHostEnvironment _environment;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public UserDashboardModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment,
            INotyfService toastNotification)
        {
            _toastNotification = toastNotification;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if(await _userManager.IsInRoleAsync(existingUser, "Admin"))
            {
                return RedirectToPage("/Admin/Dashboard");
            }
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
            AppUser.Points = existingUser.Points;

            for (var i=0; i < allRoles.Count; i++)
            {
                AppUser.Roles += allRoles[i];
                if(i != allRoles.Count-1)
                {
                    AppUser.Roles += " | ";
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.GetUserAsync(User);
                if(existingUser == null)
                {
                    _toastNotification.Error("User Not Found");
                    return RedirectToPage("./Login");
                }
                var imgUrl = "";
                if (Upload != null)
                {
                    if(Upload.ContentType=="image/jpeg" || Upload.ContentType == "image/jpg" || Upload.ContentType == "image/png")
                    {
                        var uploadsFolder = "uploads/images";
                        var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                        var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                        using var fileStream = new FileStream(imagePath, FileMode.Create);
                        await Upload.CopyToAsync(fileStream);
                        imgUrl = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                    }
                    else
                    {
                        _toastNotification.Error("Upload Only Jpeg Or Png File Types.");
                        return Page();
                    }
                }
                else
                {
                    imgUrl = existingUser.ProfilePicURL;
                }
                existingUser.Email = AppUser.Email;
                existingUser.PostalCode = AppUser.PostalCode;
                existingUser.BlockNumber = AppUser.BlockNumber;
                existingUser.UnitNumber = AppUser.UnitNumber;
                existingUser.RoadName = AppUser.RoadName;
                existingUser.PhoneNumber = AppUser.PhoneNumber;
                existingUser.IsDisabled = AppUser.IsDisabled;
                existingUser.ProfilePicURL = imgUrl;
                existingUser.FullName = AppUser.FullName;
                existingUser.Points = AppUser.Points;

                var res = await _userManager.UpdateAsync(existingUser);
                if (res.Succeeded)
                {
                    _toastNotification.Success("Profile Updated Successfully");
                }
                else
                {
                    _toastNotification.Error("Profile Update Failed");
                }
                return RedirectToPage("./UserDashboard");
            }
            _toastNotification.Error("Profile Update Failed");
            return Page();
        }
    }
}

using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nvyro.Models;
using Nvyro.Models.DTO;

namespace Nvyro.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class UserManagementModel : PageModel
    {

        [BindProperty]
        public List<AdminUserManagement> ListOfUsers { get; set; } = new List<AdminUserManagement>();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _toastNotification;
        [BindProperty]
        public static List<string> AllRoles { get; set; } = new List<string>();

        public UserManagementModel(UserManager<ApplicationUser> userManager, INotyfService toastNotification,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> OnGetAsync (int page = 1)
        {
            AllRoles = await _roleManager.Roles.Select(r=>r.Name).ToListAsync();
            int pageSize = 10;
            var users = await _userManager.Users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                ListOfUsers.Add(new AdminUserManagement { User = user, Roles = roles });
            }

            return Page();
        }
    }
}

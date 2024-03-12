using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nvyro.Models;
using Nvyro.Models.DTO;
using System.Text;

namespace Nvyro.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserManagementModel : PageModel
    {
        private static List<ApplicationUser> PriorFilteredBank { get; set; } = new List<ApplicationUser>();
        [BindProperty]
        public List<AdminUserManagement> ListOfUsers { get; set; } = new List<AdminUserManagement>();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _toastNotification;
        public static List<ApplicationUser> TotalListOfUsers { get; set; } = new List<ApplicationUser>();
        [BindProperty]
        public static List<string> AllRoles { get; set; } = new List<string>();
        [BindProperty]
        public static int PageCount { get; set; }
        [BindProperty]
        public static int CurrentPage { get; set; }
        [BindProperty]
        public static int PageSize { get; set; } = 10;
        [BindProperty]
        public static string Filter { get; set; } = "None";

        public UserManagementModel(UserManager<ApplicationUser> userManager, INotyfService toastNotification,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1, bool refresh = true)
        {
            if (refresh)
            {
                CurrentPage = pageNumber;
                AllRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

                TotalListOfUsers = await _userManager.Users.ToListAsync();
                PriorFilteredBank = TotalListOfUsers;

                var roundUp = Math.Ceiling((double)TotalListOfUsers.Count() / PageSize);
                PageCount = Convert.ToInt32(roundUp);

                var users = TotalListOfUsers.Skip((pageNumber - 1) * PageSize).Take(PageSize);

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRole = "";
                    if (roles.Count > 0)
                    {
                        userRole = roles[0];
                    }
                    ListOfUsers.Add(new AdminUserManagement { User = user, Role = userRole });
                }
            }
            else
            {
                CurrentPage = pageNumber;

                var users = TotalListOfUsers.Skip((pageNumber - 1) * PageSize).Take(PageSize);

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRole = "";
                    if (roles.Count > 0)
                    {
                        userRole = roles[0];
                    }
                    ListOfUsers.Add(new AdminUserManagement { User = user, Role = userRole });
                }

            }

            return Page();
        }

        public async Task<IActionResult> OnGetFilterAsync(string role)
        {
            Filter = role;
            if (role == "None")
            {
                TotalListOfUsers = PriorFilteredBank;
            }
            else
            {
                var filteredUsers = new List<ApplicationUser>();
                foreach (var u in PriorFilteredBank)
                {
                    if (await _userManager.IsInRoleAsync(u, role))
                    {
                        filteredUsers.Add(u);
                    }
                };
                TotalListOfUsers = filteredUsers;
            }
            var roundUp = Math.Ceiling((double)TotalListOfUsers.Count / PageSize);
            PageCount = Convert.ToInt32(roundUp);
            return Redirect("./UserManagement?refresh=false");
        }
        public IActionResult OnPostPageSize(int pageSize)
        {
            PageSize = pageSize;
            var roundUp = Math.Ceiling((double)TotalListOfUsers.Count / PageSize);
            PageCount = Convert.ToInt32(roundUp);
            return Redirect("./UserManagement?refresh=false");
        }
        public async Task<IActionResult> OnPostSearchAsync(string searchQuery)
        {
            Filter = "None";
            ListOfUsers = new List<AdminUserManagement>();
            CurrentPage = 1;
            if (searchQuery?.Length > 0)
            {

                searchQuery = searchQuery.Trim().ToUpper();
                TotalListOfUsers = await _userManager.Users.Where(u => u.NormalizedEmail.Contains(searchQuery) || u.FullName.ToUpper().Contains(searchQuery)).ToListAsync();
                PriorFilteredBank = TotalListOfUsers;

                var roundUp = Math.Ceiling((double)TotalListOfUsers.Count() / PageSize);
                PageCount = Convert.ToInt32(roundUp);

                var displayUsers = TotalListOfUsers.Take(PageSize).ToList();

                foreach (var user in displayUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRole = "";
                    if (roles.Count > 0)
                    {
                        userRole = roles[0];
                    }
                    ListOfUsers.Add(new AdminUserManagement { User = user, Role = userRole });
                }
            }
            else
            {
                var roundUp = Math.Ceiling((double)_userManager.Users.Count() / PageSize);
                PageCount = Convert.ToInt32(roundUp);
                TotalListOfUsers = await _userManager.Users.ToListAsync();
                PriorFilteredBank = TotalListOfUsers;

                var users = TotalListOfUsers.Skip((1 - 1) * PageSize).Take(PageSize);

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRole = "";
                    if (roles.Count > 0)
                    {
                        userRole = roles[0];
                    }
                    ListOfUsers.Add(new AdminUserManagement { User = user, Role = userRole });
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDownloadExcelAsync()
        {
            var fileName = "Nvyro Users_" + Guid.NewGuid() + ".csv";

            StringBuilder csv = new StringBuilder("Full Name,Email,Phone Number,Role,Disabled,Locked");

            foreach (var u in TotalListOfUsers)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var userRole = "";
                if (roles.Count > 0)
                {
                    userRole = roles[0];
                }
                bool isLocked = u.LockoutEnd > DateTime.Now;
                csv.Append($"\n{u.FullName},{u.Email},{u.PhoneNumber},{userRole},{u.IsDisabled},{isLocked}");
            };

            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(csv);
            writer.Flush();
            memoryStream.Position = 0;

            return File(memoryStream, "text/csv", $"{fileName}");

        }
    }
}
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CategoryService _categoryService;

        public AdminController(UserManager<ApplicationUser> userManager, INotyfService toastNotification, CategoryService categoryService)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _categoryService = categoryService;
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var existUser = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(existUser);
            if (existUser != null)
            {
                return Ok(new
                {
                    success = true,
                    user = existUser,
                    roles = roles
                });

            }
            return Ok(new
            {
                success = false
            });
        }
        [HttpPost("user/{userId}")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateRequestBody body, string userId)
        {
            var existUser = await _userManager.FindByIdAsync(userId);
            if (existUser != null)
            {
                var existEmail = await _userManager.FindByEmailAsync(body.Email);
                if (existEmail?.Id != existUser.Id)
                {
                    _toastNotification.Error($"Email: {body.Email}, Already Exist.");
                    return Ok(new
                    {
                        success = false,
                        message = $"Email: {body.Email}, Already Exist."
                    });
                }
                var ppUrl = body.ProfilePicUrl == "/assets/images/default-profile-pic.jpg" ? null : body.ProfilePicUrl;

                // Update the User
                existUser.ProfilePicURL = ppUrl;
                existUser.Email = body.Email;
                existUser.FullName = body.FullName;
                existUser.PhoneNumber = body.PhoneNumber;
                existUser.UnitNumber = body.UnitNumber;
                existUser.BlockNumber = body.BlockNumber;
                existUser.PostalCode = body.PostalCode;
                existUser.RoadName = body.RoadName;

                var updateRes = await _userManager.UpdateAsync(existUser);
                // Change The Role

                if (updateRes.Succeeded)
                {
                    var currentRoles = await _userManager.GetRolesAsync(existUser);
                    var removeRoleRes = await _userManager.RemoveFromRolesAsync(existUser, currentRoles);
                    if (removeRoleRes.Succeeded)
                    {
                        var changeRoleRes = await _userManager.AddToRoleAsync(existUser, body.Role);
                        if (changeRoleRes.Succeeded)
                        {
                            var newRoles = await _userManager.GetRolesAsync(existUser);
                            _toastNotification.Success($"Updated {existUser.FullName}");
                            return Ok(new
                            {
                                success = true,
                                message = $"{existUser.FullName} is updated.",
                                user = new
                                {
                                    id = existUser.Id,
                                    fullname = existUser.FullName,
                                    email = existUser.Email,
                                    phoneNumber = existUser.PhoneNumber,
                                    role = newRoles.Count > 0 ? newRoles[0] : null
                                }
                            });
                        }
                    }
                }
            }
            _toastNotification.Error($"Fail to update the user");
            return Ok(new
            {
                success = false,
                message = $"Fail to update the user."
            });
        }

        [HttpPost("quick/{userId}")]
        public async Task<IActionResult> QuickUpdateUserAsync([FromBody] QuickUpdateRequestBody body, string userId)
        {
            var existUser = await _userManager.FindByIdAsync(userId);
            if (existUser != null)
            {
                // Update the User
                existUser.LockoutEnd = body.Locked == "Yes" ? DateTime.Now + TimeSpan.FromDays(36500) : DateTime.Now - TimeSpan.FromDays(1);
                existUser.IsDisabled = body.Disabled == "Yes" ? true : false;

                var updateRes = await _userManager.UpdateAsync(existUser);
                // Change The Role

                if (updateRes.Succeeded)
                {
                    var currentRoles = await _userManager.GetRolesAsync(existUser);
                    var removeRoleRes = await _userManager.RemoveFromRolesAsync(existUser, currentRoles);
                    if (removeRoleRes.Succeeded)
                    {
                        var changeRoleRes = await _userManager.AddToRoleAsync(existUser, body.Role);
                        if (changeRoleRes.Succeeded)
                        {
                            var newRoles = await _userManager.GetRolesAsync(existUser);
                            // calculate the locked for frontend to display yes or no
                            var isLocked = existUser.LockoutEnd < DateTime.Now ? false : true;
                            _toastNotification.Success($"Updated {existUser.FullName}");
                            return Ok(new
                            {
                                success = true,
                                message = $"{existUser.FullName} is updated.",
                                user = new
                                {
                                    id = existUser.Id,
                                    disabled = existUser.IsDisabled,
                                    locked = isLocked,
                                    role = newRoles.Count > 0 ? newRoles[0] : null
                                }
                            });
                        }
                    }
                }
            }
            _toastNotification.Error($"Fail to update the user");
            return Ok(new
            {
                success = false,
                message = $"Fail to update the user."
            });
        }

        [HttpGet("charts")]
        public async Task<IActionResult> GetChartsAsync()
        {
            var recyclers = await _userManager.GetUsersInRoleAsync("Recycler");
            var organizers = await _userManager.GetUsersInRoleAsync("Organizer");

            var currentDate = DateTime.Now;

            var activeRec6mAgo = recyclers.Where(u => currentDate.AddMonths(-6) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-5)).Count();
            var activeOrg6mAgo = organizers.Where(u => currentDate.AddMonths(-6) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-5)).Count();

            var activeRec5mAgo = recyclers.Where(u => currentDate.AddMonths(-5) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-4)).Count();
            var activeOrg5mAgo = organizers.Where(u => currentDate.AddMonths(-5) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-4)).Count();

            var activeRec4mAgo = recyclers.Where(u => currentDate.AddMonths(-4) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-3)).Count();
            var activeOrg4mAgo = organizers.Where(u => currentDate.AddMonths(-4) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-3)).Count();

            var activeRec3mAgo = recyclers.Where(u => currentDate.AddMonths(-3) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-2)).Count();
            var activeOrg3mAgo = organizers.Where(u => currentDate.AddMonths(-3) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-2)).Count();

            var activeRec2mAgo = recyclers.Where(u => currentDate.AddMonths(-2) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-1)).Count();
            var activeOrg2mAgo = organizers.Where(u => currentDate.AddMonths(-2) <= u.LastActivityTimeStamp && u.LastActivityTimeStamp < currentDate.AddMonths(-1)).Count();

            var activeRec1mAgo = recyclers.Where(u => currentDate.AddMonths(-1) <= u.LastActivityTimeStamp).Count();
            var activeOrg1mAgo = organizers.Where(u => currentDate.AddMonths(-1) <= u.LastActivityTimeStamp).Count();

            var newRec6mAgo = recyclers.Where(u => currentDate.AddMonths(-6) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-5)).Count();
            var newOrg6mAgo = organizers.Where(u => currentDate.AddMonths(-6) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-5)).Count();

            var newRec5mAgo = recyclers.Where(u => currentDate.AddMonths(-5) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-4)).Count();
            var newOrg5mAgo = organizers.Where(u => currentDate.AddMonths(-5) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-4)).Count();

            var newRec4mAgo = recyclers.Where(u => currentDate.AddMonths(-4) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-3)).Count();
            var newOrg4mAgo = organizers.Where(u => currentDate.AddMonths(-4) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-3)).Count();

            var newRec3mAgo = recyclers.Where(u => currentDate.AddMonths(-3) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-2)).Count();
            var newOrg3mAgo = organizers.Where(u => currentDate.AddMonths(-3) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-2)).Count();

            var newRec2mAgo = recyclers.Where(u => currentDate.AddMonths(-2) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-1)).Count();
            var newOrg2mAgo = organizers.Where(u => currentDate.AddMonths(-2) <= u.CreatedDate && u.CreatedDate < currentDate.AddMonths(-1)).Count();

            var newRec1mAgo = recyclers.Where(u => currentDate.AddMonths(-1) <= u.CreatedDate).Count();
            var newOrg1mAgo = organizers.Where(u => currentDate.AddMonths(-1) <= u.CreatedDate).Count();


            return Ok(new
            {
                success = true,
                data = new
                {
                    activeRec6mAgo,
                    activeOrg6mAgo,
                    activeRec5mAgo,
                    activeOrg5mAgo,
                    activeRec4mAgo,
                    activeOrg4mAgo,
                    activeRec3mAgo,
                    activeOrg3mAgo,
                    activeRec2mAgo,
                    activeOrg2mAgo,
                    activeRec1mAgo,
                    activeOrg1mAgo,
                    newRec1mAgo,
                    newRec2mAgo,
                    newRec3mAgo,
                    newRec4mAgo,
                    newRec5mAgo,
                    newRec6mAgo,
                    newOrg1mAgo,
                    newOrg2mAgo,
                    newOrg3mAgo,
                    newOrg4mAgo,
                    newOrg5mAgo,
                    newOrg6mAgo
                }

            });
        }
        [HttpPost("disable/{catId}")]
        public async Task<IActionResult> PostDisablerAsync(string catId)
        {
            try
            {
                var isSuccess = await _categoryService.DisableRecycleCategoryAsync(catId);
                if (isSuccess == 1)
                {
                    _toastNotification.Success($"Successfully Enabled Category");
                }
                else if (isSuccess == 2)
                {
                    _toastNotification.Success($"Successfully Disabled Category");
                }
                else
                {
                    _toastNotification.Error($"Failed To Disable Category");
                }
                return Ok(new
                {
                    success = true,
                    data = isSuccess
                });
            }
            catch (Exception)
            {
                _toastNotification.Error($"Failed To Disable Category");
                return Ok(new
                {
                    success = false
                });
            }
        }

        public class UpdateRequestBody
        {
            public string? ProfilePicUrl { get; set; }
            public string Role { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string UnitNumber { get; set; } = string.Empty;
            public string BlockNumber { get; set; } = string.Empty;
            public string PostalCode { get; set; } = string.Empty;
            public string RoadName { get; set; } = string.Empty;

        }

        public class QuickUpdateRequestBody
        {
            public string Role { get; set; } = string.Empty;
            public string Disabled { get; set; } = string.Empty;
            public string Locked { get; set; } = string.Empty;
        }
    }
}
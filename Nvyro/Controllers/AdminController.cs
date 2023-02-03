using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nvyro.Models;

namespace Nvyro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _toastNotification;

        public AdminController(UserManager<ApplicationUser> userManager, INotyfService toastNotification)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var existUser = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(existUser);
            if(existUser != null)
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
                        success= false,
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
                                message=$"{existUser.FullName} is updated.",
                                user = new {
                                    id = existUser.Id,
                                    fullname= existUser.FullName,
                                    email = existUser.Email,
                                    phoneNumber = existUser.PhoneNumber,
                                    role = newRoles.Count > 0 ? newRoles[0]:null
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
                existUser.LockoutEnd = body.Locked=="Yes"?DateTime.Now + TimeSpan.FromDays(36500):DateTime.Now - TimeSpan.FromDays(1);
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
                                    locked= isLocked,
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

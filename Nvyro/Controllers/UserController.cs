using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nvyro.Models;
using Nvyro.Services;
using System.Collections;
using System.Text.RegularExpressions;

namespace Nvyro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("register/1/{isExternal}")]
        public async Task<IActionResult> OnNextPost([FromBody] InitialRequest iR, bool isExternal = false)
        { 
            try
            {
                var res = await _userManager.FindByEmailAsync(iR.Email);
                if (res!=null)
                {
                    return Ok(new
                    {
                        success = false,
                        errors = new ArrayList() { new { type = "Email", message=$"This Email {iR.Email} already exist" } }
                    });
                }
                else
                {
                    if (isExternal)
                    {
                        return Ok(new
                        {
                            success = true
                        });
                    }
                    var errors = new ArrayList();
                    var hasNumber = new Regex(@"[0-9]+");
                    var hasUpperChar = new Regex(@"[A-Z]+");
                    var hasLowerChar = new Regex(@"[a-z]+");
                    var hasSpecialChar = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
                    var hasMinChars = new Regex(@".{8,}");

                    if (!hasNumber.IsMatch(iR.Password))
                    {
                        //ModelState.AddModelError("MyOrganizer.Password", string.Format("Password requires at least one number"));
                        errors.Add(new {type="Password", message="Password requires at least one number"});
                    }
                    if (!hasUpperChar.IsMatch(iR.Password))
                    {
                        //ModelState.AddModelError("MyOrganizer.Password", string.Format("Password requires at least one uppercase"));
                        errors.Add(new {type="Password", message="Password requires at least one uppercase" });
                    }
                    if (!hasLowerChar.IsMatch(iR.Password))
                    {
                        //ModelState.AddModelError("MyOrganizer.Password", string.Format("Password requires at least one lowercase"));
                        errors.Add(new { type = "Password", message = "Password requires at least one lowercase" });
                    }
                    if (!hasSpecialChar.IsMatch(iR.Password))
                    {
                        //ModelState.AddModelError("MyOrganizer.Password", string.Format("Password requires at least one special character"));
                        errors.Add(new { type="Password", message="Password requires at least one special character" });
                    }
                    if (!hasMinChars.IsMatch(iR.Password))
                    {
                        //ModelState.AddModelError("MyOrganizer.Password", string.Format("Password requires at least 8 characters"));
                        errors.Add(new { type = "Password", message = "Password requires at least 8 characters" });
                    }
                    if (string.IsNullOrEmpty(iR.Email))
                    {
                        errors.Add(new { type = "Email", message = "Email is a required field." });
                    }
                    if (errors.Count > 0)
                    {
                        return Ok(new
                        {
                            success = false,
                            errors = errors
                        });
                    }
                    return Ok(new{ 
                        success = true
                    });
                }
                
            }
            catch (Exception)
            {
                return BadRequest("Failed to retrieve.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
    public class InitialRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

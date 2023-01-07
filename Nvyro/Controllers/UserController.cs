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
        private readonly UserAuthenticationService _userAuthService;
        public UserController(UserAuthenticationService userAuthService)
        {
            _userAuthService = userAuthService;
        }
        [HttpPost("1")]
        public async Task<IActionResult> OnNextPost([FromBody]InitialRequest iR)
        { 
            try
            {
                var res = await _userAuthService.GetUser(iR.Email);
                if (res.StatusCode == 1)
                {
                    return Ok(new
                    {
                        success = false,
                        errors = new ArrayList() { new { type = "Email", message=$"This Email {iR.Email} already exist" } }
                    });
                }
                else
                {
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
            await _userAuthService.LogoutAsync();
            return Redirect("/Index");
        }
    }
    public class InitialRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

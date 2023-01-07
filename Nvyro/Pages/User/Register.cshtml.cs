using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Nvyro.Models;
using Nvyro.Models.DTO;
using Nvyro.Services;
using System.Text;
using System.Text.Encodings.Web;

namespace Nvyro.Pages.User
{
   
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegistrationModel regUser { get; set; }
        private readonly UserAuthenticationService _userAuthService;
        private readonly INotyfService _toastNotification;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailSender _emailSender;

        public RegisterModel(UserAuthenticationService userAuthService, INotyfService toastNotification,UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, EmailSender emailSender)
        {
            _userAuthService = userAuthService;
            _toastNotification = toastNotification;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }   
        public void OnGet()
        {
            Console.WriteLine($"OnGet");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            try
            {
                returnUrl ??= Url.Content("~/");
                //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                var userExist = await _userManager.FindByEmailAsync(regUser.Email);
                if (userExist != null)
                {
                    _toastNotification.Error($"This Email {regUser.Email} Already Exist");
                    return Page();
                }
                if (!ModelState.IsValid)
                {
                    _toastNotification.Error("Account Creation Failed");
                    return Page();
                }
                // Create User
                ApplicationUser newUser = new ApplicationUser()
                {
                    Email = regUser.Email,
                    PostalCode = regUser.PostalCode,
                    UserName = regUser.Email,
                    BlockNumber = regUser.BlockNumber,
                    UnitNumber = regUser.UnitNumber,
                    RoadName = regUser.RoadName,
                    PhoneNumber = regUser.PhoneNumber,
                    FullName = regUser.FullName,
                    TwoFactorEnabled = true
                };
                var result = await _userManager.CreateAsync(newUser, regUser.Password);
                if (!result.Succeeded)
                {

                    _toastNotification.Error($"User Account Creation Failed");
                    return Page();
                }
                if (!await _roleManager.RoleExistsAsync(regUser.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(regUser.Role));
                    await _userManager.AddToRoleAsync(newUser, regUser.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, regUser.Role);
                }

                
                //var result = await _userAuthService.AddUser(regUser); //put model.Role in html page

                var userId = await _userManager.GetUserIdAsync(newUser);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
              
                var callbackUrl = Url.Page(
                    "/User/Login",
                    pageHandler: null,
                    values: new { userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(regUser.Email, "Welcome to NVYRO",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                
                    _toastNotification.Success("User Created Successfully. Please verify it");
                    return RedirectToPage("/User/RegisterConfirmation", new { email = regUser.Email, returnUrl = returnUrl });
            }
            catch(ArgumentNullException nullEx)
            {
                _toastNotification.Error("Account Creation Failed");
                return Page();
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DbUpdateConcurrencyException) 
            {
                _toastNotification.Error("Account Creation Failed");
                return Page();
            }
        }
       
    
    }
}

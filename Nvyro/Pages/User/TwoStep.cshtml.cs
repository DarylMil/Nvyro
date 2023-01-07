using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Models.DTO;

namespace Nvyro.Pages.User
{
    public class TwoStep : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailSender _emailSender;

        private readonly INotyfService _toastNotification;
        [BindProperty]
        public TwoStepModel TwoStepModel { get; set; }
        
        public TwoStep(UserManager<ApplicationUser> userManager, INotyfService toastNotification, EmailSender emailSender, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            
            if(user == null)
            {
                _toastNotification.Error("Error while logging in.");
                return RedirectToPage("/User/Login");
            }
            TwoStepModel = new TwoStepModel
            {
                UserId = user.Id
            };
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                _toastNotification.Error("Error while logging in.");
                return RedirectToPage("/User/Login");
            }
            var token = _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            await _emailSender.SendEmailAsync(user.Email, "2FA Token", 
                $"Your One Time Password (OTP) is: <br><h2>{token.Result}</h2><br>The password is only valid for 6 minutes.");
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            var result = await _signInManager.TwoFactorSignInAsync("Email", TwoStepModel.TwoFactorCode, false, false);
            if (result.Succeeded)
            {
                //checked if the user is admin
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin)
                {
                    return RedirectToPage("/Admin/Dashboard");
                }
                _toastNotification.Success("Succesfully LoggedIn");
                return RedirectToPage("/Index");
            }else if (result.IsLockedOut)
            {
                _toastNotification.Error("Account Is Locked");
                return Page();
            }
            _toastNotification.Error("Invalid Login Attempt");
            return Page();
        }
    }
}

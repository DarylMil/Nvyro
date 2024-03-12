using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Nvyro.Models;
using Nvyro.Models.DTO;
using System.Text;

namespace Nvyro.Pages.User
{
    public class UnlockAccount : PageModel
    {
        [BindProperty]
        public UnlockAccountModel UnlockAccountModel { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly EmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UnlockAccount(UserManager<ApplicationUser> userManager, INotyfService toastNotification, EmailSender emailSender,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> OnGetAsync(string code = null, string id = null)
        {
            if (code == null || id == null)
            {
                _toastNotification.Success("A code or email must be supplied for unlocking account.");
                return RedirectToPage("./ForgotPassword");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _toastNotification.Success("A One Time Password (OTP) is sent to your email address");
                return Page();
            }
            var result = await _userManager.VerifyUserTokenAsync(user, "Email", "UnlockAccount", Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)));
            if (result)
            {
                var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
                UnlockAccountModel = new UnlockAccountModel
                {
                    UserId = id
                };
                if (!providers.Contains("Email"))
                {
                    _toastNotification.Error("Error while logging in.");
                    return RedirectToPage("/User/Login");
                }
                _toastNotification.Success("A One Time Password (OTP) is sent to your email address");
                var token = _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                Console.WriteLine(token.Result);
                await _emailSender.SendEmailAsync(user.Email, "OTP Token",
                    $"Your One Time Password (OTP) is: <br><h2>{token.Result}</h2><br>The password is only valid for 6 minutes.");
                return Page();
            }
            else
            {
                _toastNotification.Success("A One Time Password (OTP) is sent to your email address");
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(UnlockAccountModel.UserId);
                if (user == null)
                {
                    _toastNotification.Error("Unlock Account Unsuccesfull");
                    return Page();
                }
                var result = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", UnlockAccountModel.OTP);
                if (result)
                {
                    _toastNotification.Success("Succesfully Unlock Account");
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromDays(1));
                    return RedirectToPage("/User/Login");
                }
                _toastNotification.Error("Unlock Account Unsuccesfull");
                return Page();
            }
            _toastNotification.Error("Unlock Account Unsuccesfull");
            return Page();
        }
    }
}
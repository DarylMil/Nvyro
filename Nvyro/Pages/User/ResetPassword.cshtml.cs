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
    public class ResetPassword : PageModel
    {
        [BindProperty]
        public ResetPasswordModel ResetPasswordModel { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly EmailSender _emailSender;
        public ResetPassword(UserManager<ApplicationUser> userManager, INotyfService toastNotification, EmailSender emailSender)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _emailSender = emailSender;
        }

        public IActionResult OnGet(string code = null, string email = null)
        {
            if (code == null || email == null)
            {
                _toastNotification.Error("A code or email must be supplied for password reset.");
                return RedirectToPage("./ForgotPassword");
            }
            else
            {
                ResetPasswordModel = new ResetPasswordModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                    Email = email
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var user = await _userManager.FindByEmailAsync(ResetPasswordModel.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                _toastNotification.Error("Reset Password Fail");
                return RedirectToPage("./Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, ResetPasswordModel.Code, ResetPasswordModel.Password);
            
            if (result.Succeeded)
            {
                _toastNotification.Success("Reset Password Success");
                return RedirectToPage("./Login");
            }
            _toastNotification.Error("Reset Password Fail");
            return Page();
        }
    }
}

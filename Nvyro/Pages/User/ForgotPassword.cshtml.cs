using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Nvyro.Models;
using Nvyro.Models.DTO;
using System.Text;
using System.Text.Encodings.Web;

namespace Nvyro.Pages.User
{
    public class ForgotPassword : PageModel
    {
        [BindProperty]
        public ForgotPasswordModel ForgotPasswordModel { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly EmailSender _emailSender;
        public ForgotPassword(UserManager<ApplicationUser> userManager, INotyfService toastNotification, EmailSender emailSender)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _emailSender = emailSender;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(ForgotPasswordModel.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    _toastNotification.Information("A reset email has been sent to your email address.");
                    return Page();
                }
                
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/User/ResetPassword",
                    pageHandler: null,
                    values: new { code , email = user.Email},
                    protocol: Request.Scheme);
                Console.WriteLine(callbackUrl);
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                _toastNotification.Information("A reset email has been sent to your email address.");
                //return RedirectToPage("./ForgotPassword");
                return Page();
            }

            return Page();
        }
    }
}

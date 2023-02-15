using AspNetCore.ReCaptcha;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Nvyro.Models;
using Nvyro.Models.DTO;
using Nvyro.Services;
using System.Text;
using System.Text.Encodings.Web;

namespace Nvyro.Pages.User
{
    [ValidateReCaptcha]
    [BindProperties]
    public class LoginModel : PageModel
    {
        public LoggingInModel Login { get; set; }
        private readonly INotyfService _toastNotification;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSender _emailSender;

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public LoginModel(INotyfService toastNotification, UserManager<ApplicationUser> userManager, EmailSender emailSender,
            SignInManager<ApplicationUser> signInManager)
        {
            _toastNotification = toastNotification;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            ReturnUrl = Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (userId == null || code == null)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _toastNotification.Error($"Unable to load the user");
                return Page(); 
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                _toastNotification.Success("Thank you for confirming your email");
            }
            else {
                _toastNotification.Error($"Error confirming your email");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ReturnUrl = Url.Content("~/");
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                var status = new Status();

                var user = await _userManager.FindByEmailAsync(Login.Email);

                if (user == null)
                {
                    _toastNotification.Error("Incorrect Login Credentials");
                    return Page();
                }
                if (!user.EmailConfirmed)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                   "/User/Login",
                   pageHandler: null,
                   values: new { userId = user.Id, code = code },
                   protocol: Request.Scheme);

                    var sendResult = await _emailSender.SendEmailAsync(user.Email, "Welcome to NVYRO",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (sendResult)
                    {
                        _toastNotification.Information("Please Follow Instructions Sent To Your Email");
                        return Page();
                    }
                    else
                    {
                        _toastNotification.Error("Error Sending Email. Please Try Again.");
                        return Page();
                    }
                }

                if (user.IsDisabled)
                {
                    _toastNotification.Error("Your Account Has Been Disabled. Please Contact System Admin.");
                    return Page();
                }
                var signInRes = await _signInManager.PasswordSignInAsync(Login.Email, Login.Password, false, true);
                
                if ( signInRes.IsLockedOut)
                {
                    _toastNotification.Error("Account Locked. Please Follow Instructions Sent To Your Email To Unlock.");
                    var code = await _userManager.GenerateUserTokenAsync(user, "Email","UnlockAccount");
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var url = Url.Page("/User/UnlockAccount", pageHandler: null, values:new { code=code, id = user.Id } ,protocol: Request.Scheme);
                    await _emailSender.SendEmailAsync(Login.Email, "Account Locked", $"Your account is locked due to multiple incorrect attempts to log in. Please unlock and reset your password" +
                        $" by clicking here:  <a href='{HtmlEncoder.Default.Encode(url)}'>clicking here</a>.");
                    return RedirectToPage("./LockedAccount");
                }
                if (signInRes.RequiresTwoFactor)
                {
               
                    return RedirectToPage("/User/TwoStep", new { Login.Email });
                }
                if (!signInRes.Succeeded)
                {
                    _toastNotification.Error("Incorrect Login Credentials");
                    return Page();
                }
                return Page();

            }
            catch (Exception)
            {
                _toastNotification.Error("Login Failed");
                return Page();
            }
        }
    }
}

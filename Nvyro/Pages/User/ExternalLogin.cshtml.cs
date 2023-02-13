using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Nvyro.Models;
using Nvyro.Models.DTO;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Nvyro.Pages.User
{
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _toastNotification;
        private readonly EmailSender _emailSender;

        public ExternalLoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
           INotyfService toastNotification, RoleManager<IdentityRole> roleManager, EmailSender emailSender)
        {
            _emailSender = emailSender;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public RegisterExternalModels RegisterExternalModels { get; set; }

        public string ProviderDisplayName { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        public async Task<IActionResult> OnGetCallbackAsync(string remoteError = null)
        {
            if (remoteError != null)
            {
                _toastNotification.Error($"Error from external provider: {remoteError}");
                return RedirectToPage("./Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _toastNotification.Error("Error loading external login information.");
                return RedirectToPage("./Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
            if (result.RequiresTwoFactor)
            {
                return LocalRedirect("/User/TwoStep");
            }
            if (result.IsLockedOut)
            {
                var user = await _userManager.FindByEmailAsync(RegisterExternalModels.Email);
                _toastNotification.Error("Account Locked. Please Follow Instructions Sent To Your Email To Unlock.");
                var code = await _userManager.GenerateUserTokenAsync(user, "Email", "UnlockAccount");
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var url = Url.Page("/User/UnlockAccount", pageHandler: null, values: new { code = code, id = user.Id }, protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(RegisterExternalModels.Email, "Account Locked", $"Your account is locked due to multiple incorrect attempts to log in. Please unlock and reset your password" +
                    $" by clicking here:  <a href='{HtmlEncoder.Default.Encode(url)}'>clicking here</a>.");
                return RedirectToPage("./LockedAccount");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    var existUserEmail = await _userManager.FindByEmailAsync(email);
                    if (existUserEmail != null)
                    {
                        _toastNotification.Error("Already has Account Associated With This Email. Please Login With Password Instead.");
                        return RedirectToPage("./Login");
                    }
                    RegisterExternalModels = new RegisterExternalModels()
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                if(info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                {
                    RegisterExternalModels.FullName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync()
        {
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _toastNotification.Error("Error Loading External Login Information During Confirmation.");
                return RedirectToPage("./Login");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FullName = RegisterExternalModels.FullName,
                    PostalCode = RegisterExternalModels.PostalCode,
                    BlockNumber = RegisterExternalModels.BlockNumber,
                    UnitNumber = RegisterExternalModels.UnitNumber,
                    RoadName = RegisterExternalModels.RoadName,
                    PhoneNumber = RegisterExternalModels.PhoneNumber,
                    Email = RegisterExternalModels.Email,
                    UserName = RegisterExternalModels.Email,
                    EmailConfirmed = true,
                    TwoFactorEnabled = true,
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(RegisterExternalModels.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(RegisterExternalModels.Role));
                        await _userManager.AddToRoleAsync(user, RegisterExternalModels.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, RegisterExternalModels.Role);
                    }

                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        var newUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                        var resultLogin = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
                        if (resultLogin.RequiresTwoFactor)
                        {
                            return LocalRedirect("/User/TwoStep");
                        }
                        else
                        {
                            return LocalRedirect("./Login");
                        }
                    }
                }
            }
            ProviderDisplayName = info.ProviderDisplayName;
            return Page();
        }
    }
}

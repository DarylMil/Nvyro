using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Nvyro.Data;
using Nvyro.Models;
using Nvyro.Models.DTO;
using System.Security.Claims;
using System.Text;

namespace Nvyro.Services
{
    public class UserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager= userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<Status> AddUser(RegistrationModel model)
        {
            var status = new Status();
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if(userExist != null)
            {
                status.StatusCode = 0;
                status.Message = $"This Email {model.Email} Already Exist";
                return status;
            }
            // Create User
            ApplicationUser newUser = new ApplicationUser()
            {
                Email = model.Email,
                PostalCode = model.PostalCode,
                UserName = model.Email,
                BlockNumber = model.BlockNumber,
                UnitNumber = model.UnitNumber,
                RoadName = model.RoadName,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                TwoFactorEnabled = true
            };
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = $"User Account Creation Failed";
                return status;
            }
            if(!await _roleManager.RoleExistsAsync(model.Role)){
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
                await _userManager.AddToRoleAsync(newUser, model.Role);
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, model.Role);
            }
            // confirm email first no sign in
            //await _signInManager.SignInAsync(newUser, isPersistent:false);

            status.StatusCode = 1;
            status.Message = "User Created Successfully. Please verify it";
            return status;
        }
        public async Task<Status> GetUser(string email)
        {
            var status = new Status();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                status.StatusCode = 0;
            }
            else
            {
                status.StatusCode = 1;
            }
            return status;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<Status> LoginAsync(LoggingInModel model)
        {
            var status = new Status();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Incorrect Login Credentials";
                return status;
            }
            var signInRes = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
            if (signInRes.RequiresTwoFactor)
            {
                status.StatusCode = 1;
                status.Message = "Please input the OTP.";
                return status;
            }
            else if (signInRes.IsLockedOut)
            {
                status.StatusCode = -1;
                status.Message = "Account Is Locked";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Incorrect Login Credentials";
            }
            return status;
        }
        public async Task<Status> UpdateUserAsync(ClaimsPrincipal User, UpdateUserModel applicationUser)
        {
            var status = new Status();
            var existingUser = await _userManager.GetUserAsync(User);
            
            //var loggedInUser = await _userManager.FindByIdAsync(existingUser.Id);
            if (existingUser == null)
            {
                status.StatusCode = 0;
                status.Message = "User Not Found";
                return status;
            }
            existingUser.Email = applicationUser.Email;
            existingUser.PostalCode = applicationUser.PostalCode;
            existingUser.BlockNumber = applicationUser.BlockNumber;
            existingUser.UnitNumber = applicationUser.UnitNumber;
            existingUser.RoadName = applicationUser.RoadName;
            existingUser.PhoneNumber = applicationUser.PhoneNumber;
            existingUser.IsDisabled = applicationUser.IsDisabled;
            existingUser.ProfilePicURL = applicationUser.ProfilePicURL;
            existingUser.FullName = applicationUser.FullName;
            existingUser.RecycleCategories = applicationUser.RecycleCategories;
            existingUser.Points = applicationUser.Points;

            var res = await _userManager.UpdateAsync(existingUser);
            if (res.Succeeded)
            {
                status.StatusCode = 1;
                status.Message = "Updated Successfully";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Update Failed";
                return status;
            }
        }
    }
}

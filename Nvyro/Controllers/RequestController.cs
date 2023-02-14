using Microsoft.AspNetCore.Mvc;
using Nvyro.Pages.Requests;
using Nvyro.Services;
using Nvyro.Models;
using System.Collections;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Nvyro.Models.DTO;

namespace Nvyro.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestService requestService;
        private readonly INotyfService _toastNotification;
        private readonly UserManager<ApplicationUser> userManager;
        public RequestController(RequestService requestService, INotyfService ToastNotification, UserManager<ApplicationUser> userManager)
        {
            this.requestService = requestService;
            this._toastNotification = ToastNotification;
            this.userManager = userManager;
        }
        [HttpPost("1")]
        public async Task<IActionResult> OnNextPost([FromBody] InitialRequest_Request IR)
        {
            try
            {
                var existingUser = await userManager.GetUserAsync(User);
                string postalCodeAndUnit = IR.PostalCode + IR.UnitNumber;
                var status = new Status();
                var RequestExists = requestService.FindRequestifExist(postalCodeAndUnit, existingUser.Id);
                if (RequestExists != null)
                {
                    _toastNotification.Error("Seems like you already made a request to this event using this address. Try using a different address.");
                    return Ok(new { success = false });
                }
                else
                {
                    return Ok(new
                    {
                        success = true
                    });
                }


            }
            catch (Exception)
            {
                return BadRequest("Failed to retrieve.");
            }
        }

    }
    public class InitialRequest_Request
    {
        public string PostalCode { get; set; } = string.Empty;
        public string UnitNumber { get; set; } = string.Empty;
    }
}

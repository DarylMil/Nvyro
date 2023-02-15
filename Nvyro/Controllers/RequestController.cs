using Microsoft.AspNetCore.Mvc;
using Nvyro.Pages.Requests;
using Nvyro.Services;
using Nvyro.Models;
using System.Collections;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Nvyro.Models.DTO;
using System.Net;

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
                int EventIdInt = int.Parse(IR.EventId);
                var status = new Status();
                var RequestExists = requestService.FindRequestifExist(IR.PostalCode, IR.UnitNumber, existingUser.Id , EventIdInt);
                if (RequestExists)
                {
                    _toastNotification.Error("Seems like you already made a request to this event using this address. Try using a different address.");
                    return Ok(new { success = false });
                }
                if(!ReqCheck(IR.EventStartPostal , IR.EventEndPostal, IR.PostalCode))
                {
                    _toastNotification.Error("Seems like your request's address is outside of the stated event's area.");
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
        public bool ReqCheck(string StartPostalCode, string EndPostalCode, string PostalCode)
        {
            int startPostalCode = int.Parse(StartPostalCode);
            int endPostalCode = int.Parse(EndPostalCode);
            int newPostalCode = int.Parse(PostalCode);

            if (newPostalCode >= startPostalCode && newPostalCode <= endPostalCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
    public class InitialRequest_Request
    {
        public string EventStartPostal { get; set; } = string.Empty;
        public string EventEndPostal { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string UnitNumber { get; set; } = string.Empty;
        public string EventId { get; set; } = string.Empty;
    }
}

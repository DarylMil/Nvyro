using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models.DTO
{
    public class UnlockAccountModel
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string OTP { get; set; } = string.Empty;
    }
}

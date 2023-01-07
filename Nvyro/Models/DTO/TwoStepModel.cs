using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models.DTO
{
    public class TwoStepModel
    {
        [Required]
        public string TwoFactorCode { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}

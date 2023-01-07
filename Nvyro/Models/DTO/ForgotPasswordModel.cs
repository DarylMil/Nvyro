using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models.DTO
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

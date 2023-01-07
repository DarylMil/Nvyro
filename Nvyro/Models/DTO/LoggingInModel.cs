using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvyro.Models.DTO
{
    public class LoggingInModel
    {
        [Required, MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Password { get; set; } = "";
    }
}

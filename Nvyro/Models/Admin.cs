using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvyro.Models
{
    public class Admin
    {
        [Key, Required, RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])/i?", ErrorMessage = "Invalid Email Format"), MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Password { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string FullAddress { get; set; } = string.Empty;
        [Required, MaxLength(8)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsDisabled { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public string? ProfilePicURL { get; set; }
        [Required, MaxLength(50)]
        public string? FullName { get; set; }
    }
}

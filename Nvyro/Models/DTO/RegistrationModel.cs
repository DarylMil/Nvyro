using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvyro.Models.DTO
{
    public class RegistrationModel
    {
        [Required, MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required, RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage="Minimum length of 8. Contains 1 uppercase, 1 lowercase, 1 special character and 1 digit.")]
        [Column(TypeName = "nvarchar(max)")]
        public string Password { get; set; } = "";
        [Required, MaxLength(6)]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; } = string.Empty;
        [Required, MaxLength(12)]
        [DisplayName("Block Number")]
        public string BlockNumber { get; set; } = string.Empty;
        [Required, MaxLength(6)]
        [DisplayName("Unit Number")]
        public string UnitNumber { get; set; } = string.Empty;
        [DisplayName("Road Name"), MaxLength(100), Required]
        public string RoadName { get; set; } = string.Empty;
        [Required, MinLength(8), MaxLength(8)]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string FullName { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvyro.Models
{

    public class ApplicationUser : IdentityUser
    {
        //[Required, MaxLength(50), DataType(DataType.EmailAddress)]
        //[EmailAddress]
        //public string Email { get; set; } = string.Empty;
        //[Required, DataType(DataType.Password)]
        //[Column(TypeName = "nvarchar(max)")]
        //public string Password { get; set; } = string.Empty;
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
        override public string PhoneNumber { get; set; } = string.Empty;
        public bool IsDisabled { get; set; } = false;
        public string? ProfilePicURL { get; set; }
        [Required, MaxLength(50)]
        public string FullName { get; set; } = string.Empty;
        public List<RecycleCategory>? RecycleCategories { get; set; }
        public int Points { get; set; } = 0;
        public List<Request>? Requests { get; set; }
    }
}

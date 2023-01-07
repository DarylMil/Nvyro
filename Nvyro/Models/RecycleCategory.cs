using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models
{
    public class RecycleCategory
    {
        [Key]
        public int CategoryId { get; set; } 
        [Required, MaxLength(30)]
        public string CategoryName { get; set; } = string.Empty;
        public List<ApplicationUser>? ApplicationUser { get; set; }
    }
}

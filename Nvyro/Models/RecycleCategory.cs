using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models
{
    public class RecycleCategory
    {
        [Key]
        public string CategoryId { get; set; } = Guid.NewGuid().ToString();
        [Required, MaxLength(30)]
        public string CategoryName { get; set; } = string.Empty;
        public bool IsDisabled { get; set; } = false;

    }
}

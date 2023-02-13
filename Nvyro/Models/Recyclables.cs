using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models
{
    public class Recyclables
    {
        [Required, MaxLength(30)]
        [Key]
        public string Recyclable { get; set; } = string.Empty;
    }
}

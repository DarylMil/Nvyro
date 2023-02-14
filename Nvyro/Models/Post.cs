using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(100, ErrorMessage = "Title must be at least 3 characters long.", MinimumLength = 3)]
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description must be at least 5 characters long.", MinimumLength = 5)]
        public string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;


namespace Nvyro.Models
{
    public class AddReward
    {
        public string RewardID { get; set; }

        [Required, MaxLength(30)]
        [DataType(DataType.Text)]
        public string RewardName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string RewardDescription { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public int availableQuantity { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public int requiredPoints { get; set; }

        [Required]
        public IFormFile? PhotoPath { get; set; }
    }
}

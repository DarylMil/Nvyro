using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models
{
    public class Reward
    {
        [Key]
        public string RewardID { get; set; }
        [Required, MaxLength(30)]
        public string RewardName { get; set; } = string.Empty;

        public string RewardDescription { get; set; } = string.Empty;

        public string availableQuantity { get; set; }

        public string requiredPoints { get; set; }

        public string? RewardPicURL { get; set; }
    }
}

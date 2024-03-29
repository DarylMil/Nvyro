﻿using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models
{
    public class Reward
    {
        [Key]
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
        public string? RewardPicURL { get; set; }
    }
}

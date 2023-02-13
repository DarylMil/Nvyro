using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models.DTO
{
    public class AdminUserManagement
    {
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public string Role { get; set; } = string.Empty;
        
    }
}

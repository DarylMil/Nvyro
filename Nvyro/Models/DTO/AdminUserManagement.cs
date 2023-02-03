using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models.DTO
{
    public class AdminUserManagement
    {
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public IList<string> Roles { get; set; } = new List<string>();
        
    }
}

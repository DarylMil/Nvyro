using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Nvyro.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class StatisticsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

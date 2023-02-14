using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models.DTO;
using Nvyro.Models;
using Nvyro.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;


namespace Nvyro.Pages.Forum
{
    public class AddModel : PageModel
    {
        private readonly PostService _postService;
        public AddModel(PostService postService)
        {
            _postService = postService;
        }

        [BindProperty]
        public Post Post { get; set; }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _postService.AddPost(Post);
                return Redirect("/Forum/ForuMain");
            }
            return Page();
        }
    }
}

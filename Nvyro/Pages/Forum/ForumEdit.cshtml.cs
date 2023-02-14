using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;
using System.ComponentModel.DataAnnotations;

namespace Nvyro.Pages.Forum
{
    public class ForumEditModel : PageModel
    {
        private readonly PostService _postService;
        public ForumEditModel(PostService postService)
        {
            _postService = postService;
        }

        [BindProperty]
        public Post Post { get; set; }

        public IActionResult OnGet(string id)
        {
            Post = _postService.GetPostById(id);
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _postService.UpdatePost(Post);
                return Redirect("/Forum/ForuMain");
            }
            return Page();
        }
    }
}


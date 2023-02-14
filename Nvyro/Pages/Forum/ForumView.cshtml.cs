using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.Forum
{
    public class ForumViewModel : PageModel
    {
        private readonly PostService _postService;
        public ForumViewModel(PostService postService)
        {
            _postService = postService;
        }

        public Post Post { get; set; }

        public IActionResult OnGet(string id)
        {
            Post? post = _postService.GetPostById(id);

            return Page();
        }
    }
}
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Nvyro.Models;
using Nvyro.Services;

namespace Nvyro.Pages.Forum
{
    public class ForumViewModel : PageModel
    {
        private readonly PostService _postService;
        private readonly INotyfService _toastNotification;
        private IWebHostEnvironment _environment;

        public ForumViewModel(PostService postService, INotyfService toastNotification, IWebHostEnvironment environment)
        {
            _postService = postService;
            _toastNotification = toastNotification;
            _environment = environment;
        }

        public Post Post { get; set; }

        public IActionResult OnGet(string id)
        {
            Post? post = _postService.GetPostById(id);
            if (post != null)
            {
                Post = post;
                return Page();
            }
            else
            {
                _toastNotification.Error("Post not found");
                return Redirect("/Forum/ForuMain");
            }
        }
    }
}
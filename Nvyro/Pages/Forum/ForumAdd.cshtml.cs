using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Nvyro.Models;
using Nvyro.Services;



namespace Nvyro.Pages.Forum
{
    public class AddModel : PageModel
    {
        private readonly PostService _postService;
        private readonly INotyfService _toastNotification;
        private IWebHostEnvironment _environment;
        public AddModel(PostService postService, INotyfService toastNotification, IWebHostEnvironment environment)
        {
            _postService = postService;
            _toastNotification = toastNotification;
            _environment = environment;
        }

        [BindProperty]
        public Post Post { get; set; }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _postService.AddPost(Post);
                _toastNotification.Success("Post created!");
                return Redirect("/Forum/ForuMain");
            }
            return Page();
        }
    }
}

using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models;
using Nvyro.Services;


namespace Nvyro.Pages.Forum
{
    public class ForumModel : PageModel
    {
        private readonly PostService _postService;
        private readonly INotyfService _toastNotification;
        private IWebHostEnvironment _environment;

        [BindProperty]
        public int id { get; set; }
        public string sortOrder { get; set; }

        public ForumModel(PostService postService, INotyfService toastNotification, IWebHostEnvironment environment)
        {
            _postService = postService;
            _toastNotification = toastNotification;
            _environment = environment;

        }

        [BindProperty]
        public List<Post> Posts { get; set; } = new();

        //public void OnGet()
        //{
        //    Posts = _postService.GetAll();
        //}

        public void OnGet(string sortOrder)
        {
            this.sortOrder = sortOrder;
            switch (sortOrder)
            {
                case "Alphabetical":
                    Posts = _postService.GetAll().OrderBy(p => p.Title).ToList();
                    break;
                case "Latest":
                    Posts = _postService.GetAll().OrderByDescending(p => p.PostDate).ToList();
                    break;
                case "Earliest":
                    Posts = _postService.GetAll().OrderBy(p => p.PostDate).ToList();
                    break;
                default:
                    Posts = _postService.GetAll();
                    break;
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            var post = _postService.GetPostById(id.ToString());
            if (post != null)
            {
                _postService.DeletePost(post);
                _toastNotification.Success("Post successfully deleted");
                return RedirectToPage("/Forum/ForuMain");
            }
            else
            {
                _toastNotification.Error("Post not found");
                return RedirectToPage("/Forum/ForuMain");
            }
        }
    }
}

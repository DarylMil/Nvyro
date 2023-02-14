using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Nvyro.Models;
using Nvyro.Services;
using System.ComponentModel.DataAnnotations;

namespace Nvyro.Pages.Forum
{
    public class ForumEditModel : PageModel
    {
        private readonly PostService _postService;
        private readonly INotyfService _toastNotification;
        private IWebHostEnvironment _environment;

        public ForumEditModel(PostService postService, INotyfService toastNotification, IWebHostEnvironment environment)
        {
            _postService = postService;
            _toastNotification = toastNotification;
            _environment = environment;
        }

        [BindProperty]
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

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _postService.UpdatePost(Post);
                _toastNotification.Success("Post successfully changed");
                return Redirect("/Forum/ForuMain");
            }
            return Page();
        }

        //public IActionResult OnDelete(int id)
        //{
        //    Post? post = _postService.Posts.FirstOrDefault(
        //    x => x.Id.Equals(id));
        //    if (post != null)
        //    {
        //        _postService.Posts.Remove(post);
        //        _postService.SaveChanges();
        //        _toastNotification.Success("Post successfully deleted");
        //        return Redirect("/Forum/ForuMain");
        //    }
        //    else
        //    {
        //        _toastNotification.Error("Post not found");
        //        return Redirect("/Forum/ForuMain");
        //    }
        //}

        public IActionResult OnDelete(string id)
        {
            int parsedId;
            if (int.TryParse(id, out parsedId))
            {
                _postService.DeletePost(parsedId);
                _toastNotification.Success("Post successfully deleted");
                return Redirect("/Forum/ForuMain");
            }
            else
            {
                _toastNotification.Error("Post not found");
                return Redirect("/Forum/ForuMain");
            }
        }
    }
}


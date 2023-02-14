using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nvyro.Models.DTO;
using Nvyro.Models;
using Nvyro.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace Nvyro.Pages.Forum
{
    public class ForumModel : PageModel
    {
        private readonly PostService _postService;
        
        public ForumModel(PostService postService)
        {
            _postService = postService;
        }
        public List<Post> Posts { get; set; } = new();
        public void OnGet()
        {
            Posts = _postService.GetAll();
        }
    }
}

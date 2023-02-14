using Nvyro.Data;
using Nvyro.Models;

namespace Nvyro.Services
{
    public class PostService
    {
        private readonly MyDbContext _context;
        public PostService(MyDbContext context)
        {
            _context = context;
        }
        public List<Post> GetAll()
        {
            return _context.Posts.OrderBy(d => d.Title).ToList();
        }
        //public Post? GetPostById(string id)
        //{
        //    Post? post = _context.Posts.FirstOrDefault(
        //    x => x.Id.Equals(id));
        //    return post;
        //}

        public Post? GetPostById(string id)
        {
            int parsedId;
            if (int.TryParse(id, out parsedId))
            {
                Post? post = _context.Posts.FirstOrDefault(
                x => x.Id.Equals(parsedId));
                return post;
            }
            else
            {
                return null;
            }
        }

        public void AddPost(Post post)
        {
            post.PostDate = DateTime.Now;
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
        public void UpdatePost(Post post)
        {
            post.PostDate = DateTime.Now;
            _context.Posts.Update(post);
            _context.SaveChanges();
        }

        public void DeletePost(int id)
        {
            Post? post = _context.Posts.FirstOrDefault(
            x => x.Id.Equals(id));
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }
    }
}

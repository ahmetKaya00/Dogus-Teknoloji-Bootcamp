using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers{

    public class PostsController : Controller{

        private IPostRepository _postRrepository;
        private ICommentRepository _commentRepository;
        public PostsController(IPostRepository postRrepository,ICommentRepository commentRepository){
            _postRrepository = postRrepository;
            _commentRepository = commentRepository;
        }
        public async Task<IActionResult> Index(string tag){
            var claims = User.Claims;
            var posts = _postRrepository.Posts;

            if(!string.IsNullOrEmpty(tag)){
                posts = posts.Where(x=>x.Tags.Any(t=>t.Url == tag));
            }
            return View(new PostViewModel{Posts = await posts.ToListAsync()});
        }

        public async Task<IActionResult> Details(string url){
            return View(await _postRrepository.Posts.Include(x=>x.Tags).Include(x=>x.Comments).ThenInclude(x=>x.User).FirstOrDefaultAsync(p=>p.Url ==url));
        }

        [HttpPost]
        public JsonResult AddComment(int PostId, string UserName, string Text){
            var entity = new Comment{
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                User = new User {UserName = UserName, Image = "p1.jpg"}
            };
            _commentRepository.CreateComment(entity);
            return Json(new {
                UserName,
                Text,
                entity.PublishedOn,
                entity.User.Image
            });
        }
    }
}
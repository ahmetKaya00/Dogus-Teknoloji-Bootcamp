using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
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
        public JsonResult AddComment(int PostId, string Text){
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);
            var entity = new Comment{
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = int.Parse(userId ?? "")
            };
            _commentRepository.CreateComment(entity);
            return Json(new {
                username,
                Text,
                entity.PublishedOn,
                avatar
            });
        }

        [Authorize]
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model){
            if(ModelState.IsValid){
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _postRrepository.CreatePost(
                    new Post{
                        Title = model.Title,
                        Content = model.Content,
                        Url = model.Url,
                        UserId = int.Parse(userId ?? ""),
                        PublishedOn = DateTime.Now,
                        Image = "1.jpg",
                        IsActive = false
                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult>List(){
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRrepository.Posts;

            if(string.IsNullOrEmpty(role)){
                posts = posts.Where(i=>i.UserId == userId);
            }

            return View(await posts.ToListAsync());
        }
        [Authorize]
        public IActionResult Edit(int? id){
            if(id==null){
                return NotFound();
            }

            var post = _postRrepository.Posts.FirstOrDefault(i=>i.PostId == id);
            if(post == null){
                return NotFound();
            }

            return View(new PostCreateViewModel{
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(PostCreateViewModel model){
            if(ModelState.IsValid){
                var entityToUpdate = new Post{
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url
                };
                if(User.FindFirstValue(ClaimTypes.Role) == "admin"){
                    entityToUpdate.IsActive = model.IsActive;
                }
                _postRrepository.EditPost(entityToUpdate);
                return RedirectToAction("List");
            }
            return View(model);
        }
    }
}
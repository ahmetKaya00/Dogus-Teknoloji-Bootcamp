using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers{

    public class PostsController : Controller{

        private IPostRepository _postRrepository;
        public PostsController(IPostRepository postRrepository){
            _postRrepository = postRrepository;
        }
        public IActionResult Index(){
            return View(
                new PostViewModel{
                    Posts = _postRrepository.Posts.ToList()
                }
            );
        }

        public async Task<IActionResult> Details(int? id){
            return View(await _postRrepository.Posts.FirstOrDefaultAsync(p=>p.PostId ==id));
        }
    }
}
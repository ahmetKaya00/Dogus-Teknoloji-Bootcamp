using Microsoft.EntityFrameworkCore;
using BlogApp.Entity;
namespace BlogApp.Data.Concrete.EfCore{

    public static class SeedData{

        public static void TestVerileriniDoldur(IApplicationBuilder app){
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if(context != null){
                if(context.Database.GetPendingMigrations().Any()){
                context.Database.Migrate();
            }
            if(!context.Tags.Any()){
                context.Tags.AddRange(
                    new Tag{Text = "web programlama", Url = "web-programlama", Color = TagColors.primary},
                    new Tag{Text = "backend", Url = "backend", Color = TagColors.danger},
                    new Tag{Text = "frontend", Url = "frontend", Color = TagColors.secondary},
                    new Tag{Text = "game", Url= "game", Color = TagColors.success},
                    new Tag{Text = "fullstack",Url = "full-stack", Color = TagColors.warning}
                );
                context.SaveChanges();
            }
            if(!context.Users.Any()){
                context.Users.AddRange(
                    new User {UserName = "ahmetkaya",Image = "p1.jpg", Name = "Ahmet Kaya", Email = "info@ahmetkaya.com",Password="123456"},
                    new User {UserName = "siartemur", Image = "p2.jpg", Name = "Siar Temur", Email = "info@siartemur.com",Password="123456"}
                );
                context.SaveChanges();
            }
            if(!context.Posts.Any()){
                context.Posts.AddRange(
                    new Post{
                        Title = "Asp.net Core Bootcamp",
                        Content = "Asp.net core dersleri başladı.",
                        Url = "aspnet-core-bootcamp",
                        IsActive = true,
                        Image = "1.jpg",
                        PublishedOn = DateTime.Now.AddDays(-10),
                        Tags = context.Tags.Take(3).ToList(),
                        UserId = 1,
                        Comments = new List<Comment>{
                            new Comment {Text = "Başarılı bir şekilde başlamadı",PublishedOn = new DateTime(),UserId =2},
                            new Comment {Text = "Başarılı bir şekilde başladı",PublishedOn = new DateTime(),UserId =1},
                        }
                    },
                    new Post{
                        Title = "Doğus Teknoloji Bootcamp",
                        Content = "Derslerimiz başladı.",
                        Url = "dogus-teknoloji-bootcamp",
                        IsActive = true,
                        Image = "2.jpg",
                        PublishedOn = DateTime.Now.AddDays(-20),
                        Tags = context.Tags.Take(2).ToList(),
                        UserId = 1
                    },
                    new Post{
                        Title = "Backend Bootcamp",
                        Content = "Derslerimiz başladı.",
                        Url = "backend-bootcamp",
                        IsActive = true,
                        Image = "3.jpg",
                        PublishedOn = DateTime.Now.AddDays(-25),
                        Tags = context.Tags.Take(4).ToList(),
                        UserId = 2
                    }
                );
                context.SaveChanges();
            }
        }
    }
    }
}
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
                    new Tag{Text = "web programlama"},
                    new Tag{Text = "backend"},
                    new Tag{Text = "frontend"},
                    new Tag{Text = "game"},
                    new Tag{Text = "fullstack"}
                );
                context.SaveChanges();
            }
            if(!context.Users.Any()){
                context.Users.AddRange(
                    new User {UserName = "ahmetkaya"},
                    new User {UserName = "siartemur"}
                );
                context.SaveChanges();
            }
            if(!context.Posts.Any()){
                context.Posts.AddRange(
                    new Post{
                        Title = "Asp.net Core Bootcamp",
                        Content = "Asp.net core dersleri başladı.",
                        IsActive = true,
                        Image = "1.jpg",
                        PublishedOn = DateTime.Now.AddDays(-10),
                        Tags = context.Tags.Take(3).ToList(),
                        UserId = 1
                    },
                    new Post{
                        Title = "Doğus Teknoloji Bootcamp",
                        Content = "Derslerimiz başladı.",
                        IsActive = true,
                        Image = "2.jpg",
                        PublishedOn = DateTime.Now.AddDays(-20),
                        Tags = context.Tags.Take(2).ToList(),
                        UserId = 1
                    },
                    new Post{
                        Title = "Backend Bootcamp",
                        Content = "Derslerimiz başladı.",
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
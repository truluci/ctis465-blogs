using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Blogs.APP.Domain
{
    // Provides a factory for creating BlogsDb instances at design time.
    // This is used by Entity Framework (EF) tools such as
    // migrations to construct the database context when the
    // application is not running.
    // This class may need to be created if there are any exceptions
    // during Scaffolding.
    public class BlogsDbFactory : IDesignTimeDbContextFactory<BlogsDb>
    {
        // The connection string in the configuration file
        // (appsettings.json).
        const string CONNECTIONSTRING = "data source=BlogsDB.db";


        // Creates a new instance of the BlogsDb type using the connection
        // string. This method is called by EF tooling at design time.
        public BlogsDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogsDb>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new BlogsDb(optionsBuilder.Options);
        }



        // Seeds the database tables with initial data (Optional).
        public void SeedDb()
        {
            // Create a new instance of the BlogsDb type:
            var db = CreateDbContext(null);

            // Delete existing data:
            var blogTags = db.BlogTags.ToList();
            db.BlogTags.RemoveRange(blogTags);
            var blogs = db.Blogs.ToList();
            db.Blogs.RemoveRange(blogs);
            var tags = db.Tags.ToList();
            db.Tags.RemoveRange(tags);
            db.SaveChanges();

            // Reset identity columns (for SQLite):
            // IDs will start from 1
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='BlogTags'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Blogs'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Tags'");

            // Insert new data:
            // Tags:
            db.Tags.Add(new Tag { Name = "Technology" });
            db.Tags.Add(new Tag { Name = "Programming" });
            db.Tags.Add(new Tag { Name = "Travel" });
            db.Tags.Add(new Tag { Name = "Food" });
            db.Tags.Add(new Tag { Name = "Sports" });
            db.SaveChanges(); // commit changes to the database

            // Blogs with BlogTags:
            // UserId and UserName values match the seeded users of the Users microservice.
            db.Blogs.Add(new Blog
            {
                Title = "Getting Started with Microservices",
                Content = "Microservices is an architectural style that structures an application " +
                    "as a collection of small, independently deployable services.",
                Rating = 4.5m,
                PublishDate = new DateTime(2026, 10, 1, 10, 30, 0),
                IsArchived = false,
                UserId = 1,
                UserName = "admin",
                BlogTags = new List<BlogTag>
                {
                    new BlogTag { TagId = db.Tags.Single(t => t.Name == "Technology").Id },
                    new BlogTag { TagId = db.Tags.Single(t => t.Name == "Programming").Id }
                }
            });
            db.Blogs.Add(new Blog
            {
                Title = "MediatR and CQRS in .NET",
                Content = "MediatR helps us implement the mediator pattern by separating " +
                    "requests from their handlers.",
                Rating = 4m,
                PublishDate = new DateTime(2026, 10, 15, 14, 0, 0),
                IsArchived = false,
                UserId = 1,
                UserName = "admin",
                BlogTags = new List<BlogTag>
                {
                    new BlogTag { TagId = db.Tags.Single(t => t.Name == "Programming").Id }
                }
            });
            db.Blogs.Add(new Blog
            {
                Title = "A Weekend in Cappadocia",
                Content = "Hot air balloons, fairy chimneys and the best pottery kebab I have ever had.",
                Rating = 5m,
                PublishDate = new DateTime(2026, 9, 20, 9, 15, 0),
                IsArchived = false,
                UserId = 2,
                UserName = "user",
                BlogTags = new List<BlogTag>
                {
                    new BlogTag { TagId = db.Tags.Single(t => t.Name == "Travel").Id },
                    new BlogTag { TagId = db.Tags.Single(t => t.Name == "Food").Id }
                }
            });
            db.Blogs.Add(new Blog
            {
                Title = "Old Football Notes",
                Content = "Some notes about last season's matches.",
                PublishDate = new DateTime(2025, 5, 3, 21, 45, 0),
                IsArchived = true,
                UserId = 2,
                UserName = "user",
                BlogTags = new List<BlogTag>
                {
                    new BlogTag { TagId = db.Tags.Single(t => t.Name == "Sports").Id }
                }
            });
            db.SaveChanges();
        }
    }
}

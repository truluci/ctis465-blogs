using Microsoft.EntityFrameworkCore;

namespace Blogs.APP.Domain
{
    public class BlogsDb : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }

        public BlogsDb(DbContextOptions options) : base(options)
        {
        }
    }
}

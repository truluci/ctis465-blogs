using CORE.Domain;

namespace Blogs.APP.Domain
{
    public class BlogTag : Record
    {
        public int BlogId { get; set; } // foreign key

        public Blog Blog { get; set; } // navigation property

        public int TagId { get; set; } // foreign key

        public Tag Tag { get; set; } // navigation property
    }
}

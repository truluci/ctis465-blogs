using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace Blogs.APP.Domain
{
    public class Tag : Record
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        public List<BlogTag> BlogTags { get; set; } = new(); // navigation property
    }
}

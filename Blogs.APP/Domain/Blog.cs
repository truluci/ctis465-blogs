using CORE.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogs.APP.Domain
{
    public class Blog : Record
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public decimal? Rating { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsArchived { get; set; }

        // UserName and UserId come from the Users microservice,
        // therefore there is no foreign key relationship with a User entity in this database.
        [StringLength(30)]
        public string UserName { get; set; }

        public int? UserId { get; set; }

        public List<BlogTag> BlogTags { get; set; } = new(); // navigation property

        [NotMapped]
        public List<int> TagIds
        {
            get => BlogTags.Select(bt => bt.TagId).ToList();
            set => BlogTags = value.Select(tId => new BlogTag
            {
                TagId = tId
            }).ToList();
        }
    }
}

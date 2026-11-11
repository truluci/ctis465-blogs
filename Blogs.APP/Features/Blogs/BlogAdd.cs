using Blogs.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Blogs.APP.Features.Blogs
{
    public class BlogAddRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(150, MinimumLength = 3,
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string Content { get; set; }

        [Range(0, 5, ErrorMessage = "{0} must be between {1} and {2}!")]
        public decimal? Rating { get; set; }

        // If not sent, the current date and time will be used.
        public DateTime? PublishDate { get; set; }

        public bool IsArchived { get; set; }

        // UserName and UserId will be taken from the JWT in the final phase (Authentication).
        [StringLength(30, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string UserName { get; set; }

        public int? UserId { get; set; }

        public List<int> TagIds { get; set; } = new();
    }

    public class BlogAddHandler : DbService<Blog>, IRequestHandler<BlogAddRequest, CommandResponse>
    {
        public BlogAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BlogAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(b => b.Title == request.Title.Trim(), cancellationToken))
                return Error("Blog with the same title exists!");
            var entity = new Blog
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                Rating = request.Rating,
                PublishDate = request.PublishDate ?? DateTime.Now,
                IsArchived = request.IsArchived,
                UserName = request.UserName?.Trim(),
                UserId = request.UserId,
                TagIds = request.TagIds
            };
            await DbAddAsync(entity, cancellationToken);
            return Success(entity.Id, "Blog added successfully.");
        }
    }
}

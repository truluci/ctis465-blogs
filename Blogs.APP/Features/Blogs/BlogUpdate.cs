using Blogs.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Blogs.APP.Features.Blogs
{
    public class BlogUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(150, MinimumLength = 3,
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string Content { get; set; }

        [Range(0, 5, ErrorMessage = "{0} must be between {1} and {2}!")]
        public decimal? Rating { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public DateTime? PublishDate { get; set; }

        public bool IsArchived { get; set; }

        // UserName and UserId will be taken from the JWT in the final phase (Authentication).
        [StringLength(30, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string UserName { get; set; }

        public int? UserId { get; set; }

        public List<int> TagIds { get; set; } = new();
    }

    public class BlogUpdateHandler : DbService<Blog>, IRequestHandler<BlogUpdateRequest, CommandResponse>
    {
        public BlogUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Blog> DbQuery()
        {
            return base.DbQuery().Include(b => b.BlogTags);
        }

        public async Task<CommandResponse> Handle(BlogUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(b => b.Id != request.Id &&
                b.Title == request.Title.Trim(), cancellationToken))
                return Error("Blog with the same title exists!");
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Blog not found!");
            // Remove the old relations with tags, new relations will be created through TagIds
            DbRemove(entity.BlogTags);
            entity.Title = request.Title.Trim();
            entity.Content = request.Content.Trim();
            entity.Rating = request.Rating;
            entity.PublishDate = request.PublishDate.Value;
            entity.IsArchived = request.IsArchived;
            entity.UserName = request.UserName?.Trim();
            entity.UserId = request.UserId;
            entity.TagIds = request.TagIds;
            await DbUpdateAsync(entity, cancellationToken);
            return Success(entity.Id, "Blog updated successfully.");
        }
    }
}

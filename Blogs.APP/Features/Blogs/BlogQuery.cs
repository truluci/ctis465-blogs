using Blogs.APP.Domain;
using Blogs.APP.Features.Tags;
using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogs.APP.Features.Blogs
{
    public class BlogQueryRequest : Record, IRequest<IQueryable<BlogQueryResponse>>
    {
    }

    public class BlogQueryResponse : Record
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public decimal? Rating { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsArchived { get; set; }

        public string UserName { get; set; }

        public int? UserId { get; set; }

        public List<int> TagIds { get; set; }

        public string RatingR { get; set; }

        public string PublishDateR { get; set; }

        public string IsArchivedR { get; set; }

        // Way 1:
        public string TagNamesR { get; set; }

        // Way 2:
        public List<TagQueryResponse> TagsR { get; set; }
    }

    public class BlogQueryHandler : DbService<Blog>, IRequestHandler<BlogQueryRequest, IQueryable<BlogQueryResponse>>
    {
        public BlogQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Blog> DbQuery()
        {
            // Note: SQLite doesn't support ordering by decimal columns (Rating),
            // therefore blogs are ordered by publish date and title.
            return base.DbQuery().Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                .OrderBy(b => b.IsArchived)
                .ThenByDescending(b => b.PublishDate)
                .ThenBy(b => b.Title);
        }

        public Task<IQueryable<BlogQueryResponse>> Handle(BlogQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbQuery().Select(b => new BlogQueryResponse
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                Rating = b.Rating,
                PublishDate = b.PublishDate,
                IsArchived = b.IsArchived,
                UserName = b.UserName,
                UserId = b.UserId,
                TagIds = b.TagIds,

                RatingR = b.Rating.HasValue ? b.Rating.Value.ToString("N1") : string.Empty,
                PublishDateR = b.PublishDate.ToString("MM/dd/yyyy HH:mm:ss"),
                IsArchivedR = b.IsArchived ? "Archived" : "Published",
                // Way 1:
                TagNamesR = string.Join(", ", b.BlogTags.Select(bt => bt.Tag.Name)),
                // Way 2:
                TagsR = b.BlogTags.OrderBy(bt => bt.Tag.Name)
                    .Select(bt => new TagQueryResponse
                    {
                        Id = bt.Tag.Id,
                        Name = bt.Tag.Name
                    }).ToList()
            });
            return Task.FromResult(query);
        }
    }
}

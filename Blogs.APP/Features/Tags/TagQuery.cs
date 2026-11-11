using Blogs.APP.Domain;
using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogs.APP.Features.Tags
{
    public class TagQueryRequest : Record, IRequest<IQueryable<TagQueryResponse>>
    {
    }

    public class TagQueryResponse : Record
    {
        public string Name { get; set; }

        public int BlogCountR { get; set; }

        public string BlogTitlesR { get; set; }
    }

    public class TagQueryHandler : DbService<Tag>, IRequestHandler<TagQueryRequest, IQueryable<TagQueryResponse>>
    {
        public TagQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Tag> DbQuery()
        {
            return base.DbQuery().Include(t => t.BlogTags).ThenInclude(bt => bt.Blog)
                .OrderBy(t => t.Name);
        }

        public Task<IQueryable<TagQueryResponse>> Handle(TagQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbQuery().Select(t => new TagQueryResponse
            {
                Id = t.Id,
                Name = t.Name,

                BlogCountR = t.BlogTags.Count,
                BlogTitlesR = string.Join(", ", t.BlogTags.Select(bt => bt.Blog.Title))
            });
            return Task.FromResult(query);
        }
    }
}

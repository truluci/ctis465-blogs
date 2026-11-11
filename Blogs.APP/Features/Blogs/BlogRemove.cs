using Blogs.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogs.APP.Features.Blogs
{
    public class BlogRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class BlogRemoveHandler : DbService<Blog>, IRequestHandler<BlogRemoveRequest, CommandResponse>
    {
        public BlogRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Blog> DbQuery() => base.DbQuery().Include(b => b.BlogTags);

        public async Task<CommandResponse> Handle(BlogRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Blog not found!");
            // Remove the relations of the blog with tags first (many to many relational entities)
            DbRemove(entity.BlogTags);
            await DbRemoveAsync(entity, cancellationToken);
            return Success(entity.Id, "Blog deleted successfully.");
        }
    }
}

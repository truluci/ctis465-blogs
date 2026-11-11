using Blogs.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogs.APP.Features.Tags
{
    public class TagRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class TagRemoveHandler : DbService<Tag>, IRequestHandler<TagRemoveRequest, CommandResponse>
    {
        public TagRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Tag> DbQuery() => base.DbQuery().Include(t => t.BlogTags);

        public async Task<CommandResponse> Handle(TagRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Tag not found!");
            // Remove the relations of the tag with blogs first (many to many relational entities)
            DbRemove(entity.BlogTags);
            await DbRemoveAsync(entity, cancellationToken);
            return Success(entity.Id, "Tag deleted successfully.");
        }
    }
}

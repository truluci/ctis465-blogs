using Blogs.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Blogs.APP.Features.Tags
{
    public class TagUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }
    }

    public class TagUpdateHandler : DbService<Tag>, IRequestHandler<TagUpdateRequest, CommandResponse>
    {
        public TagUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(t => t.Id != request.Id && t.Name == request.Name.Trim(), cancellationToken))
                return Error("Tag with the same name exists!");
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Tag not found!");
            entity.Name = request.Name.Trim();
            await DbUpdateAsync(entity, cancellationToken);
            return Success(entity.Id, "Tag updated successfully.");
        }
    }
}

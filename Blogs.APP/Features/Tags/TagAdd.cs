using Blogs.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Blogs.APP.Features.Tags
{
    public class TagAddRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }
    }

    public class TagAddHandler : DbService<Tag>, IRequestHandler<TagAddRequest, CommandResponse>
    {
        public TagAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(t => t.Name == request.Name.Trim(), cancellationToken))
                return Error("Tag with the same name exists!");
            var entity = new Tag
            {
                Name = request.Name.Trim()
            };
            await DbAddAsync(entity, cancellationToken);
            return Success(entity.Id, "Tag added successfully.");
        }
    }
}

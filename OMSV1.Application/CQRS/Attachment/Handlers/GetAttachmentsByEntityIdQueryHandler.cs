using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Domain.Entities.Attachments;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Queries.Attachments;

namespace OMSV1.Application.Handlers.Attachments
{
public class GetAttachmentsByEntityIdQueryHandler : IRequestHandler<GetAttachmentsByEntityIdQuery, List<AttachmentDto>>
{
    private readonly AppDbContext _dbContext;

    public GetAttachmentsByEntityIdQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<AttachmentDto>> Handle(GetAttachmentsByEntityIdQuery request, CancellationToken cancellationToken)
    {
        var attachments = await _dbContext.AttachmentCUs
            .Where(a => a.EntityId == request.EntityId && a.EntityType == request.EntityType)
            .Select(a => new AttachmentDto
            {
                // Include the Id, EntityId, and EntityType
                Id = a.Id, 
                EntityId = a.EntityId,
                EntityType = a.EntityType,

                // File information
                FileName = a.FileName,
                FilePath = a.FilePath // Return the FilePath (URL) to the client
            })
            .ToListAsync(cancellationToken);

        return attachments;
    }
}

}

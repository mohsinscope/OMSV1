using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Queries.Attachments
{
    public class GetAttachmentsByEntityIdQuery : IRequest<List<AttachmentDto>>
    {
        public Guid EntityId { get; set; }
        public EntityType EntityType { get; set; }

        public GetAttachmentsByEntityIdQuery(Guid entityId, EntityType entityType)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }
}

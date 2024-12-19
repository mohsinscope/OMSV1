using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Enums;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Attachments
{
    public class GetAttachmentsByEntityIdQuery : IRequest<List<AttachmentDto>>
    {
        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }

        public GetAttachmentsByEntityIdQuery(int entityId, EntityType entityType)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }
}

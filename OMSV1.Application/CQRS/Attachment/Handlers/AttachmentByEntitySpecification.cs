
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Specifications;

namespace OMSV1.Application.Specifications
{
    public class AttachmentByEntitySpecification : BaseSpecification<AttachmentCU>
    {
        public AttachmentByEntitySpecification(Guid entityId, EntityType entityType)
            : base(a => a.EntityId == entityId && a.EntityType == entityType)
        {
        }
    }
}
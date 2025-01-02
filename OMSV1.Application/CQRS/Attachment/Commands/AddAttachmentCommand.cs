using MediatR;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Attachments
{
    public class AddAttachmentCommand : IRequest<string> // or use a DTO for the response if needed
    {
        public IFormFile File { get; set; }
        public Guid EntityId { get; set; }
        public EntityType EntityType { get; set; }

        public AddAttachmentCommand(IFormFile file, Guid entityId, EntityType entityType)
        {
            File = file;
            EntityId = entityId;
            EntityType = entityType;
        }
    }
}

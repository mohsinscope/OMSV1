using MediatR;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Attachment;

public class UpdateAttachmentCommand : IRequest<bool>
    {
    public Guid AttachmentId { get; set; }
    public IFormFile NewPhoto { get; set; }  // Add this property for the new photo
    public Guid EntityId { get; set; }
    public EntityType EntityType { get; set; }
    }


            
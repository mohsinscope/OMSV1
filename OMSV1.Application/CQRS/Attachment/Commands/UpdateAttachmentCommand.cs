using MediatR;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Attachment;

public class UpdateAttachmentCommand : IRequest<bool>
{
    public Guid AttachmentId { get; set; }
    public required IFormFileCollection NewPhotos { get; set; }  // Changed to IFormFileCollection for multiple files
    public Guid EntityId { get; set; }
    public EntityType EntityType { get; set; }
}
using MediatR;
using Microsoft.AspNetCore.Http;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Attachment
{
    public class AddAttachmentCommand : IRequest<AttachmentDto>
    {
        public IFormFile File { get; set; }
        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }
    }
}

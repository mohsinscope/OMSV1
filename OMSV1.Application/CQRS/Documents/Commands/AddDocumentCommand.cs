using MediatR;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Documents
{
    public class AddDocumentWithAttachmentCommand : IRequest<Guid>
    {
        // Document properties
        public required string DocumentNumber { get; set; }
        public required string Title { get; set; }
        public DocumentType DocumentType { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime DocumentDate { get; set; }
        public bool IsRequiresReply { get; set; }
        public Guid PartyId { get; set; }
        public string? Subject { get; set; }
        public Guid? ParentDocumentId { get; set; } = null;
        public Guid? CCId { get; set; } = null;

        // Attachment property
        public required List<IFormFile> File { get; set; }
    }
}

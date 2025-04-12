using MediatR;
using System;

namespace OMSV1.Application.Commands.Documents
{
    /// <summary>
    /// Changes the status of a Document to indicate a reply is no longer required.
    /// </summary>
    public class ChangeDocumentStatusCommand : IRequest<bool>
    {
        public Guid DocumentId { get; set; }
        public Guid UserId { get; set; }
        public string? Notes { get; set; }

        public ChangeDocumentStatusCommand(Guid documentId, Guid userId, string? notes = null)
        {
            if (documentId == Guid.Empty)
                throw new ArgumentException("Document ID cannot be empty.", nameof(documentId));
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            DocumentId = documentId;
            UserId = userId;
            Notes = notes;
        }
    }
}

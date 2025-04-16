using MediatR;
using OMSV1.Application.Commands.Documents;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Profiles; // For retrieving DocumentParty, if needed.
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using OMSV1.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documents
{
    public class UpdateDocumentDetailsCommandHandler : IRequestHandler<UpdateDocumentDetailsCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDocumentDetailsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateDocumentDetailsCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the document.
            var document = await _unitOfWork.Repository<Document>().GetByIdAsync(request.DocumentId);
            if (document == null)
                throw new KeyNotFoundException($"Document with Id {request.DocumentId} was not found.");

            // Only the original creator (by ProfileId) is allowed to update.
            if (document.ProfileId != request.ProfileId)
                throw new UnauthorizedAccessException("Only the original creator can update this document.");

            // Optionally update immutable fields if your domain design allows (for example, DocumentNumber)
            // For instance:
            // document.DocumentNumber = request.DocumentNumber;
            // In many systems, the document number is unique and unchangeable.

            // Update the document's modifiable details using the domain Update method.
            // (This method updates Title, Subject, DocumentDate, DocumentType, IsRequiresReply, and ResponseType.)
            document.Update(
                title: request.Title,
                subject: request.Subject,
                documentDate: DateTime.SpecifyKind(request.DocumentDate, DateTimeKind.Utc),
                docType: request.DocumentType,
                isRequiresReply: request.IsRequiresReply,
                responseType: request.ResponseType
            );

            // If your domain allows updating additional properties such as ParentDocumentId, ProjectId, or PartyId,
            // you can set them here. For this example, we'll assume these values remain unchanged.

            // Update the CC recipients.
            // Clear existing CCs and add those provided in the update command.
            // (This assumes that the Document entity exposes a mutable collection for CCs.)
            document.CCs.Clear();
            if (request.CCIds != null && request.CCIds.Any())
            {
                foreach (var ccId in request.CCIds)
                {
                    // Retrieve the DocumentParty entity for each ccId.
                    var ccParty = await _unitOfWork.Repository<DocumentParty>().GetByIdAsync(ccId);
                    if (ccParty != null)
                    {
                        document.CCs.Add(ccParty);
                    }
                }
            }

            // Optionally, update the optional Notes property directly if it is not handled in the domain method.
            // For example:
            // document.Notes = request.Notes;
            // (Ensure your domain design allows for this kind of update.)

            // Log the update action in DocumentHistory.
            var history = new DocumentHistory(
                documentId: document.Id,
                profileId: request.ProfileId,
                actionType: DocumentActions.Edit,
                actionDate: DateTime.UtcNow,
                notes: "Document details updated (excluding file attachments)."
            );
            await _unitOfWork.Repository<DocumentHistory>().AddAsync(history);

            // Update the document in the repository.
            await _unitOfWork.Repository<Document>().UpdateAsync(document);

            // Save changes in one transaction.
            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new Exception("Failed to update the document.");

            return document.Id;
        }
    }
}

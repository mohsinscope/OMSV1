using MediatR;
using OMSV1.Application.Commands.Documents;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using OMSV1.Infrastructure.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documents
{
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDocumentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the document.
            var document = await _unitOfWork.Repository<Document>().GetByIdAsync(request.DocumentId);
            if (document == null)
                throw new KeyNotFoundException($"Document with Id {request.DocumentId} was not found.");

            // Only the original creator (by ProfileId) is allowed to update.
            if (document.ProfileId != request.ProfileId)
                throw new UnauthorizedAccessException("Only the original creator can update this document.");

            // Update the document using a domain method.
            document.Update(
                title: request.Title,
                subject: request.Subject,
                documentDate: DateTime.SpecifyKind(request.DocumentDate, DateTimeKind.Utc),
                docType: request.DocumentType,
                isRequiresReply: request.IsRequiresReply,
                responseType: request.ResponseType
            );

            // Optionally, log the update action in a document history.
            var history = new DocumentHistory(
                documentId: document.Id,
                profileId: request.ProfileId,
                actionType: DocumentActions.Edit, // Use the "Edit" action for updates.
                actionDate: DateTime.UtcNow,
                notes: "Document updated by its creator."
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

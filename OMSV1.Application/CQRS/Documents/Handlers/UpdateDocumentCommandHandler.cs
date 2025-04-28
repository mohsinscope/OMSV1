// --- UpdateDocumentDetailsCommandHandler.cs ---
using MediatR;
using OMSV1.Application.Commands.Documents;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace OMSV1.Application.Handlers.Documents
{
    public class UpdateDocumentDetailsCommandHandler
        : IRequestHandler<UpdateDocumentDetailsCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDocumentDetailsCommandHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<Guid> Handle(
            UpdateDocumentDetailsCommand request,
            CancellationToken cancellationToken)
        {
                          // --- DUPLICATE CHECK ---
        var alreadyExists = await _unitOfWork.Repository<Document>()
            .GetAllAsQueryable()
            .AnyAsync(d => d.DocumentNumber == request.DocumentNumber, cancellationToken);
               if (alreadyExists)
        throw new DuplicateDocumentNumberException(request.DocumentNumber);

            
            // 1. Retrieve the document.
            var document = await _unitOfWork
                .Repository<Document>()
                .GetByIdAsync(request.DocumentId);
            if (document == null)
                throw new KeyNotFoundException(
                    $"Document with Id {request.DocumentId} was not found.");

            // 2. Only the original creator can update.
            if (document.ProfileId != request.ProfileId)
                throw new UnauthorizedAccessException(
                    "Only the original creator can update this document.");

            // 3. Update all modifiable fields via the domain Update method.
            document.Update(
                title:            request.Title,
                subject:          request.Subject,
                documentDate:     DateTime.SpecifyKind(request.DocumentDate, DateTimeKind.Utc),
                docType:          request.DocumentType,
                requiresReply:    request.IsRequiresReply,
                responseType:     request.ResponseType,
                isUrgent:         request.IsUrgent,
                isImportant:      request.IsImportant,
                isNeeded:         request.IsNeeded,
                notes:            request.Notes
            );

            // 4. If you need to reassign ProjectId or PartyId your domain would
            //    need dedicated methods—omitted here for brevity.

            // 5. Replace CC links using domain methods.
            //    Remove existing:
            foreach (var link in document.CcLinks.ToList())
                document.RemoveCc(link.DocumentCcId);

            //    Add new:
            if (request.CCIds.Any())
            {
                foreach (var ccId in request.CCIds)
                {
                    var ccEntity = await _unitOfWork
                        .Repository<DocumentCC>()
                        .GetByIdAsync(ccId);
                    if (ccEntity != null)
                        document.AddCc(ccEntity);
                }
            }

            // 6. Log the update in history.
            var history = new DocumentHistory(
                documentId: document.Id,
                profileId:  request.ProfileId,
                actionType: DocumentActions.Edit,
                actionDate: DateTime.UtcNow,
                notes:      "Document details updated."
            );
            await _unitOfWork
                .Repository<DocumentHistory>()
                .AddAsync(history);

            // 7. Persist changes.
            await _unitOfWork.Repository<Document>().UpdateAsync(document);
            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new Exception("Failed to update the document.");

            return document.Id;
        }
    }
}

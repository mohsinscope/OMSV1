using MediatR;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Commands.Documents
{
    public class MarkDocumentAsAuditedCommandHandler 
        : IRequestHandler<MarkDocumentAsAuditedCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkDocumentAsAuditedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            MarkDocumentAsAuditedCommand request, 
            CancellationToken cancellationToken)
        {
            // 1. Load document
            var document = await _unitOfWork
                .Repository<Document>()
                .GetByIdAsync(request.DocumentId);
            if (document == null)
                throw new KeyNotFoundException(
                    $"Document with ID {request.DocumentId} was not found.");
    // 7.x Load the editing profile
    var profile = await _unitOfWork.Repository<OMSV1.Domain.Entities.Profiles.Profile>()
        .GetByIdAsync(request.ProfileId);
    if (profile == null)
        throw new KeyNotFoundException($"Profile {request.ProfileId} not found.");
            // 2. Flip audited flag
            document.MarkAsAudited();

            // 3. Create history entry
            var history = new DocumentHistory(
                documentId: document.Id,
                profileId:  request.ProfileId,
                actionType: DocumentActions.Audited,
                actionDate: DateTime.UtcNow,
                notes:      $"تم التدقيق بوساطة المستخدم {profile.FullName}"
            );
            await _unitOfWork
                .Repository<DocumentHistory>()
                .AddAsync(history);

            // 4. Save document and history
            await _unitOfWork
                .Repository<Document>()
                .UpdateAsync(document);

            if (await _unitOfWork.SaveAsync(cancellationToken))
                return true;

            throw new Exception("Failed to mark the document as audited.");
        }
    }
}

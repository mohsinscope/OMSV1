// --- UnmarkDocumentAsAuditedCommandHandler.cs ---
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
    public class UnmarkDocumentAsAuditedCommandHandler 
        : IRequestHandler<UnmarkDocumentAsAuditedCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnmarkDocumentAsAuditedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UnmarkDocumentAsAuditedCommand request, 
            CancellationToken cancellationToken)
        {
            // 1. Load the document
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
            // 2. Flip audited flag off
            document.UnmarkAsAudited();

            // 3. Append history entry
            var history = new DocumentHistory(
                documentId: document.Id,
                profileId:  request.ProfileId,
                actionType: DocumentActions.UnAudited,
                actionDate: DateTime.UtcNow,
                notes:      $"تم إزالة التدقيق بوساطة المستخدم {profile.FullName}"
            );
            await _unitOfWork
                .Repository<DocumentHistory>()
                .AddAsync(history);

            // 4. Persist changes
            await _unitOfWork
                .Repository<Document>()
                .UpdateAsync(document);

            if (await _unitOfWork.SaveAsync(cancellationToken))
                return true;

            throw new Exception("Failed to un–audit the document.");
        }
    }
}

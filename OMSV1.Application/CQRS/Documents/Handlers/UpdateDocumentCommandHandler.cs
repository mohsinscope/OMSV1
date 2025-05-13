using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.Documents;
using OMSV1.Application.Exceptions;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.Enums;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Domain.SeedWork;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documents
{
    public class UpdateDocumentWithAttachmentCommandHandler
        : IRequestHandler<UpdateDocumentWithAttachmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        private readonly IDocumentLinkManager _documentLinkManager; // Added for handling links

        public UpdateDocumentWithAttachmentCommandHandler(
            IUnitOfWork unitOfWork,
            IPhotoService photoService,
            IDocumentLinkManager documentLinkManager) // Inject document link manager
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _documentLinkManager = documentLinkManager;
        }

        public async Task<Guid> Handle(
            UpdateDocumentWithAttachmentCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Load existing document
            var document = await _unitOfWork.Repository<Document>()
                .GetByIdAsync(request.DocumentId);
            if (document == null)
                throw new KeyNotFoundException($"Document {request.DocumentId} not found.");

            // 2. Check for duplicate document number if it's being changed
            if (request.DocumentNumber != null)
            {
                var dup = await _unitOfWork.Repository<Document>()
                    .GetAllAsQueryable()
                    .AnyAsync(d =>
                        d.DocumentNumber == request.DocumentNumber
                        && d.Id != request.DocumentId,
                        cancellationToken);
                if (dup)
                    throw new DuplicateDocumentNumberException(request.DocumentNumber);
            }

            // 3. Load the editing profile
            var profile = await _unitOfWork.Repository<OMSV1.Domain.Entities.Profiles.Profile>()
                .GetByIdAsync(request.ProfileId);
            if (profile == null)
                throw new KeyNotFoundException($"Profile {request.ProfileId} not found.");

            // 4. Patch basic document properties
            document.Patch(
                documentNumber: request.DocumentNumber,
                title: request.Title,
                subject: request.Subject,
                documentDate: request.DocumentDate,
                docType: request.DocumentType,
                requiresReply: request.IsRequiresReply,
                responseType: request.ResponseType,
                isUrgent: request.IsUrgent,
                isImportant: request.IsImportant,
                isNeeded: request.IsNeeded,
                notes: request.Notes,
                projectId: request.ProjectId,
                parentDocumentId: request.ParentDocumentId,
                ministryId: request.MinistryId,
                generalDirectorateId: request.GeneralDirectorateId,
                directorateId: request.DirectorateId,
                departmntId: request.DepartmentId,
                sectionId: request.SectionId,
                privatePartyId: request.PrivatePartyId
            );

            // Update document in the repository
            await _unitOfWork.Repository<Document>().UpdateAsync(document);
            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new Exception("Failed to update basic document properties.");

            // 5. Handle CCs through DocumentLinkManager
            if (request.CCIds != null)
            {
                // Use the dedicated service to handle CC links
                await _documentLinkManager.UpdateDocumentCcLinksAsync(
                    document.Id, 
                    request.CCIds, 
                    cancellationToken);
            }

            // 6. Handle Tags through DocumentLinkManager
            if (request.TagIds != null)
            {
                // Use the dedicated service to handle Tag links
                await _documentLinkManager.UpdateDocumentTagLinksAsync(
                    document.Id, 
                    request.TagIds, 
                    cancellationToken);
            }

            // 7. Create document history
            var history = new DocumentHistory(
                documentId: document.Id,
                profileId: request.ProfileId,
                actionType: DocumentActions.Edit,
                actionDate: DateTime.UtcNow,
                notes: $"تم التعديل بوساطة {profile.FullName}"
            );
            await _unitOfWork.Repository<DocumentHistory>().AddAsync(history);
            
            // 8. Save changes to persist history
            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new Exception("Failed to add document history.");

            // 9. Handle attachments if any new files were provided
            if (request.Files?.Any() == true)
            {
                // Load existing attachments
                var existing = await _unitOfWork
                    .Repository<DocumentAttachment>()
                    .GetAllAsQueryable()
                    .Where(a => a.DocumentId == document.Id)
                    .ToListAsync(cancellationToken);

                // Delete both file and DB record for each attachment
                foreach (var att in existing)
                {
                    // Remove the file
                    await _photoService.DeletePhotoAsync(att.FilePath);

                    // Remove the DB row
                    await _unitOfWork
                        .Repository<DocumentAttachment>()
                        .DeleteAsync(att);
                }

                // Upload & insert the new attachments
                foreach (var file in request.Files)
                {
                    var uploadResult = await _photoService
                        .AddPhotoAsync(file, document.Id, EntityType.Document);

                    var newAttachment = new DocumentAttachment(
                        filePath: uploadResult.FilePath,
                        documentId: document.Id
                    );

                    await _unitOfWork.Repository<DocumentAttachment>()
                        .AddAsync(newAttachment);
                }

                // Persist attachment changes
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new Exception("Failed to update document attachments.");
            }

            return document.Id;
        }
    }
}

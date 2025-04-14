using MediatR;
using OMSV1.Application.Commands.Documents;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using OMSV1.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documents
{
    public class ReplyDocumentWithAttachmentCommandHandler : IRequestHandler<ReplyDocumentWithAttachmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public ReplyDocumentWithAttachmentCommandHandler(IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task<Guid> Handle(ReplyDocumentWithAttachmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Retrieve the parent Document.
                var parentDoc = await _unitOfWork.Repository<Document>()
                    .GetByIdAsync(request.ParentDocumentId);
                if (parentDoc == null)
                    throw new KeyNotFoundException($"Document with ID {request.ParentDocumentId} not found.");

                // 2. Retrieve the CC DocumentParty if a valid CCId is provided.
                DocumentParty? ccParty = null;
                if (request.CCId.HasValue && request.CCId.Value != Guid.Empty)
                {
                    ccParty = await _unitOfWork.Repository<DocumentParty>()
                        .GetByIdAsync(request.CCId.Value);
                }

                // 3. Retrieve the Profile for the replying user using the provided ProfileId.
                var profile = await _unitOfWork.Repository<Profile>().GetByIdAsync(request.ProfileId);
                if (profile == null)
                    throw new KeyNotFoundException($"Profile with ID {request.ProfileId} was not found.");

                // 4. Create the reply Document using the parent's domain method.
                // Pass the user-supplied ReplyDocumentNumber into the CreateReply method.
                var replyDoc = parentDoc.CreateReply(
                    documentNumber: request.ReplyDocumentNumber,
                    replyType: request.ReplyType,
                    replyDate: request.ReplyDate,
                    requiresReply: request.RequiresReply,
                    profileId: request.ProfileId,
                    profile: profile,
                    responseType: request.ResponseType,
                    ccId: request.CCId,
                    cc: ccParty
                );

                // 5. Mark the parent document as replied.
                parentDoc.MarkAsReplied();

                // 6. Add the reply document to the repository.
                await _unitOfWork.Repository<Document>().AddAsync(replyDoc);

                // 7. Process file attachments.
                if (request.File == null || request.File.Count == 0)
                    throw new ArgumentException("No files were uploaded for the attachments.");

                foreach (var file in request.File)
                {
                    if (file != null && file.Length > 0)
                    {
                        var photoResult = await _photoService.AddPhotoAsync(file, replyDoc.Id, EntityType.Document);
                        var attachment = new AttachmentCU(
                            filePath: photoResult.FilePath,
                            entityType: EntityType.Document,
                            entityId: replyDoc.Id
                        );
                        await _unitOfWork.Repository<AttachmentCU>().AddAsync(attachment);
                    }
                }

                // 8. Create a DocumentHistory entry for the reply action using ProfileId.
                var history = new DocumentHistory(
                    documentId: parentDoc.Id,
                    profileId: request.ProfileId,
                    actionType: DocumentActions.Reply,
                    actionDate: DateTime.UtcNow,
                    notes: request.Notes
                );
                await _unitOfWork.Repository<DocumentHistory>().AddAsync(history);

                // 9. Commit all changes in one transaction.
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the reply document and its attachments to the database.");
                }

                return replyDoc.Id;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while creating the reply document with attachment.", ex);
            }
        }
    }
}

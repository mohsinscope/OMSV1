// // --- ReplyDocumentWithAttachmentCommandHandler.cs ---
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using OMSV1.Application.Commands.Documents;
// using OMSV1.Application.Exceptions;
// using OMSV1.Application.Helpers;
// using OMSV1.Domain.Entities.Attachments;
// using OMSV1.Domain.Entities.DocumentHistories;
// using OMSV1.Domain.Entities.Documents;
// using OMSV1.Domain.Entities.Ministries;
// using OMSV1.Domain.Entities.Profiles;
// using OMSV1.Domain.Enums;
// using OMSV1.Domain.SeedWork;
// using OMSV1.Infrastructure.Interfaces;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;

// namespace OMSV1.Application.Handlers.Documents
// {
//     public class ReplyDocumentWithAttachmentCommandHandler
//     : IRequestHandler<ReplyDocumentWithAttachmentCommand, Guid>
// {
//     private readonly IUnitOfWork  _unitOfWork;
//     private readonly IPhotoService _photoService;

//     public ReplyDocumentWithAttachmentCommandHandler(
//         IUnitOfWork unitOfWork,
//         IPhotoService photoService)
//     {
//         _unitOfWork   = unitOfWork;
//         _photoService = photoService;
//     }

//     public async Task<Guid> Handle(
//     ReplyDocumentWithAttachmentCommand request,
//     CancellationToken cancellationToken)
// {
//                   // --- DUPLICATE CHECK ---
//         var alreadyExists = await _unitOfWork.Repository<Document>()
//             .GetAllAsQueryable()
//             .AnyAsync(d => d.DocumentNumber == request.ReplyDocumentNumber, cancellationToken);
//                if (alreadyExists)
//         throw new DuplicateDocumentNumberException(request.ReplyDocumentNumber);

//     // 1. Load parent document
//     var parentDoc = await _unitOfWork.Repository<Document>()
//         .GetByIdAsync(request.ParentDocumentId);
//     if (parentDoc == null)
//         throw new KeyNotFoundException($"Document {request.ParentDocumentId} not found.");

//     // 2. Load party
//     var party = await _unitOfWork.Repository<DocumentParty>()
//         .GetByIdAsync(request.PartyId);
//     if (party == null)
//         throw new KeyNotFoundException($"DocumentParty {request.PartyId} not found.");

//     // 3. Load CC entities
//     var ccEntities = new List<DocumentCC>();
//     if (request.CCIds?.Any() == true)
//     {
//         ccEntities = await _unitOfWork.Repository<DocumentCC>()
//             .GetAllAsQueryable()
//             .Where(cc => request.CCIds.Contains(cc.Id))
//             .ToListAsync(cancellationToken);
//     }

//     // 4. Load ministry
//     Ministry? ministry = null;
//     if (request.MinistryId.HasValue)
//     {
//         ministry = await _unitOfWork.Repository<Ministry>()
//             .GetByIdAsync(request.MinistryId.Value);
//         if (ministry == null)
//             throw new KeyNotFoundException($"Ministry {request.MinistryId} not found.");
//     }

//     // 5. Load Tag entities
//     var tagEntities = new List<Tag>();
//     if (request.TagIds.Any())
//     {
//         tagEntities = await _unitOfWork.Repository<Tag>()
//             .GetAllAsQueryable()
//             .Where(t => request.TagIds.Contains(t.Id))
//             .ToListAsync(cancellationToken);
//     }

//     // 6. Load profile
//     var profile = await _unitOfWork.Repository<Profile>()
//         .GetByIdAsync(request.ProfileId);
//     if (profile == null)
//         throw new KeyNotFoundException($"Profile {request.ProfileId} not found.");

//     // 7. Create the reply without link rows
//     var replyDoc = parentDoc.CreateReply(
//         documentNumber:    request.ReplyDocumentNumber,
//         title:             request.Title,
//         replyType:         request.ReplyType,
//         projectId:         request.ProjectId,
//         replyDate:         DateTime.SpecifyKind(request.ReplyDate, DateTimeKind.Utc),
//         requiresReply:     request.RequiresReply,
//         partyId:           request.PartyId,
//         party:             party,
//         profileId:         request.ProfileId,
//         profile:           profile,
//         responseType:      request.ResponseType,
//         ministryId:        request.MinistryId,
//         ministry:          ministry,
//         isUrgent:          request.IsUrgent,
//         isImportant:       request.IsImportant,
//         isNeeded:          request.IsNeeded,
//         subject:           request.Subject,
//         ccs:               null,
//         tags:              null,
//         notes:             request.Notes
//     );

//     // 8. Persist the new reply (no links yet)
//     await _unitOfWork.Repository<Document>().AddAsync(replyDoc);
//     await _unitOfWork.SaveAsync(cancellationToken);

//     // 9. Seed CC and Tag link rows
// foreach (var cc in ccEntities)
// {
//     replyDoc.AddCc(cc);
//     replyDoc.CCs.Add(cc);           // also add to the skip-nav CC collection
// }
// foreach (var tag in tagEntities)
// {
//     replyDoc.AddTag(tag);
//     // no skip-nav Tags collection, TagLinks only
// }

// // 10. Mark parent as replied
//     parentDoc.MarkAsReplied();

//     // 11. Save CC/Tag links and parent update
//     await _unitOfWork.SaveAsync(cancellationToken);

//     // 12. Process attachments
//     if (request.Files == null || !request.Files.Any())
//         throw new ArgumentException("At least one file is required.");

//     foreach (var file in request.Files)
//     {
//         var photo = await _photoService
//             .AddPhotoAsync(file, replyDoc.Id, EntityType.Document);
//         var attachment = new AttachmentCU(
//             filePath:   photo.FilePath,
//             entityType: EntityType.Document,
//             entityId:   replyDoc.Id
//         );
//         await _unitOfWork.Repository<AttachmentCU>().AddAsync(attachment);
//     }

// // 13. Record history (include the replier’s full name)
// var history = new DocumentHistory(
//     documentId: replyDoc.Id,
//     profileId:  request.ProfileId,
//     actionType: DocumentActions.Reply,
//     actionDate: DateTime.UtcNow,
//     notes:      $"تم التعليق بوساطة {profile.FullName}"
// );

//     await _unitOfWork.Repository<DocumentHistory>().AddAsync(history);

//     // 14. Final save for attachments & history
//     if (!await _unitOfWork.SaveAsync(cancellationToken))
//         throw new Exception("Failed to save reply attachments, tags or history.");

//     return replyDoc.Id;
// }
//     }
// }

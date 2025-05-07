// // --- UpdateDocumentWithAttachmentCommandHandler.cs ---
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using OMSV1.Application.Commands.Documents;
// using OMSV1.Application.Exceptions;
// using OMSV1.Domain.Entities.Attachments;
// using OMSV1.Domain.Entities.DocumentHistories;
// using OMSV1.Domain.Entities.Documents;
// using OMSV1.Domain.Entities.Ministries;
// using OMSV1.Domain.Enums;
// using OMSV1.Infrastructure.Interfaces;
// using OMSV1.Domain.SeedWork;
// using AutoMapper;

// namespace OMSV1.Application.Handlers.Documents
// {
//     public class UpdateDocumentWithAttachmentCommandHandler
//         : IRequestHandler<UpdateDocumentWithAttachmentCommand, Guid>
//     {
//         private readonly IUnitOfWork _unitOfWork;
//         private readonly IPhotoService _photoService;

//         public UpdateDocumentWithAttachmentCommandHandler(
//             IUnitOfWork unitOfWork,
//             IPhotoService photoService)
//         {
//             _unitOfWork   = unitOfWork;
//             _photoService = photoService;
//         }

// public async Task<Guid> Handle(
//     UpdateDocumentWithAttachmentCommand request,
//     CancellationToken cancellationToken)
// {
//     // 1. Load existing document
//     var document = await _unitOfWork.Repository<Document>()
//         .GetByIdAsync(request.DocumentId);
//     if (document == null)
//         throw new KeyNotFoundException($"Document {request.DocumentId} not found.");

//     // 0. Only check for duplicate number if they’re actually changing it
//     if (request.DocumentNumber != null)
//     {
//         var dup = await _unitOfWork.Repository<Document>()
//             .GetAllAsQueryable()
//             .AnyAsync(d =>
//                 d.DocumentNumber == request.DocumentNumber
//                 && d.Id != request.DocumentId,
//                 cancellationToken);
//         if (dup)
//             throw new DuplicateDocumentNumberException(request.DocumentNumber);
//     }

//     // 2–6. (load party, CCs, tags, ministry as before...)
//     // 7.x Load the editing profile
//     var profile = await _unitOfWork.Repository<OMSV1.Domain.Entities.Profiles.Profile>()
//         .GetByIdAsync(request.ProfileId);
//     if (profile == null)
//         throw new KeyNotFoundException($"Profile {request.ProfileId} not found.");

//     // 7. Do a _partial_ update:
//     document.Patch(
//         documentNumber:   request.DocumentNumber,
//         title:            request.Title,
//         subject:          request.Subject,
//         documentDate:     request.DocumentDate,
//         docType:          request.DocumentType,
//         requiresReply:    request.IsRequiresReply,
//         responseType:     request.ResponseType,
//         isUrgent:         request.IsUrgent,
//         isImportant:      request.IsImportant,
//         isNeeded:         request.IsNeeded,
//         notes:            request.Notes,
//         projectId:        request.ProjectId,
//         partyId:          request.PartyId,
//         parentDocumentId: request.ParentDocumentId,
//         ministryId:       request.MinistryId
//     );

//     // 8–9. Reset CCs and Tags if lists provided (you could also skip if null)…
//     if (request.CCIds != null)   { /* clear/Add new CCs */ }
//     if (request.TagIds != null)  { /* clear/Add new Tags */ }

//     // 10–12. Attachments, history and Save as before…
//     // 10–12. Create history, attachments, etc.
//     var history = new DocumentHistory(
//         documentId: document.Id,
//         profileId:  request.ProfileId,
//         actionType: DocumentActions.Edit,
//         actionDate: DateTime.UtcNow,
//         notes:      $"تم التعديل بوساطة {profile.FullName}"
//     );

//     await _unitOfWork.Repository<Document>().UpdateAsync(document);
//     if (!await _unitOfWork.SaveAsync(cancellationToken))
//         throw new Exception("Failed to update document.");

//     return document.Id;
// }

//     }
// }

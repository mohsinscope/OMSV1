// --- AddDocumentWithAttachmentCommandHandler.cs ---
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.Documents;
using OMSV1.Application.Exceptions;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.Handlers.Documents
{
    public class AddDocumentWithAttachmentCommandHandler 
        : IRequestHandler<AddDocumentWithAttachmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public AddDocumentWithAttachmentCommandHandler(
            IUnitOfWork unitOfWork,
            IPhotoService photoService)
        {
            _unitOfWork   = unitOfWork;
            _photoService = photoService;
        }


        public async Task<Guid> Handle(
            AddDocumentWithAttachmentCommand request, 
            CancellationToken cancellationToken)
        {
            // --- DUPLICATE CHECK ---
            var exists = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                .AnyAsync(d => d.DocumentNumber == request.DocumentNumber, cancellationToken);
            if (exists)
                throw new DuplicateDocumentNumberException(request.DocumentNumber);

            // Load optional relations
            Ministry? ministry = null;
            if (request.MinistryId.HasValue)
            {
                ministry = await _unitOfWork.Repository<Ministry>()
                    .GetByIdAsync(request.MinistryId.Value);
                if (ministry == null)
                    throw new KeyNotFoundException($"Ministry {request.MinistryId} not found.");
            }

            GeneralDirectorate? generalDir = null;
            if (request.GeneralDirectorateId.HasValue)
            {
                generalDir = await _unitOfWork.Repository<GeneralDirectorate>()
                    .GetByIdAsync(request.GeneralDirectorateId.Value);
                if (generalDir == null)
                    throw new KeyNotFoundException($"GeneralDirectorate {request.GeneralDirectorateId} not found.");
            }

            Directorate? directorate = null;
            if (request.DirectorateId.HasValue)
            {
                directorate = await _unitOfWork.Repository<Directorate>()
                    .GetByIdAsync(request.DirectorateId.Value);
                if (directorate == null)
                    throw new KeyNotFoundException($"Directorate {request.DirectorateId} not found.");
            }

            Department? department = null;
            if (request.DepartmentId.HasValue)
            {
                department = await _unitOfWork.Repository<Department>()
                    .GetByIdAsync(request.DepartmentId.Value);
                if (department == null)
                    throw new KeyNotFoundException($"Department {request.DepartmentId} not found.");
            }

            Section? section = null;
            if (request.SectionId.HasValue)
            {
                section = await _unitOfWork.Repository<Section>()
                    .GetByIdAsync(request.SectionId.Value);
                if (section == null)
                    throw new KeyNotFoundException($"Section {request.SectionId} not found.");
            }

            // 2. Load PrivateParty if provided
            PrivateParty? privateParty = null;
            if (request.PrivatePartyId.HasValue)
            {
                privateParty = await _unitOfWork.Repository<PrivateParty>()
                                     .GetByIdAsync(request.PrivatePartyId.Value);
                if (privateParty == null)
                    throw new KeyNotFoundException($"PrivateParty {request.PrivatePartyId} not found.");
            }
            var profile = await _unitOfWork.Repository<Profile>()
                                .GetByIdAsync(request.ProfileId);
            if (profile == null)
                throw new KeyNotFoundException($"Profile {request.ProfileId} not found.");

            // 2. Load existing CCs
            var ccEntities = new List<DocumentCC>();
            if (request.CCIds?.Any() == true)
            {
                foreach (var ccId in request.CCIds)
                {
                    var cc = await _unitOfWork.Repository<DocumentCC>()
                                  .GetByIdAsync(ccId);
                    if (cc != null)
                        ccEntities.Add(cc);
                }
            }
            // 4. Load Tag entities (tracked) via IQueryable
            var tagEntities = new List<Tag>();
            if (request.TagIds?.Any() == true)
            {
                tagEntities = await _unitOfWork.Repository<Tag>()
                    .GetAllAsQueryable()
                    .Where(t => request.TagIds.Contains(t.Id))
                    .ToListAsync(cancellationToken);
            }

            // 5. Instantiate new Document (ccs and tags via constructor)
            var document = new Document(
                documentNumber:   request.DocumentNumber,
                title:            request.Title,
                docType:          request.DocumentType,
                projectId:        request.ProjectId,
                documentDate:     DateTime.SpecifyKind(request.DocumentDate, DateTimeKind.Utc),
                requiresReply:    request.IsRequiresReply,
                profileId:        request.ProfileId,
                profile:          profile,
                responseType:     request.ResponseType,
                ministryId: request.MinistryId,
                ministry: ministry,
                generalDirectorateId: request.GeneralDirectorateId,
                generalDirectorate: generalDir,
                directorateId: request.DirectorateId,
                directorate: directorate,
                departmentId: request.DepartmentId,
                department: department,
                sectionId: request.SectionId,
                section: section,
                privatePartyId:   request.PrivatePartyId,
                privateParty:     privateParty,
                isUrgent:         request.IsUrgent,
                isImportant:      request.IsImportant,
                isNeeded:         request.IsNeeded,
                subject:          request.Subject,
                parentDocumentId: request.ParentDocumentId,
                notes:            request.Notes
            );
            // ←— NEW: if this is itself a reply, mark it as replied
            if (request.ResponseType == ResponseType.IncomingReply
            || request.ResponseType == ResponseType.OutgoingReply)
            {
                document.MarkAsReplied();
            }

            
            await _unitOfWork.Repository<Document>().AddAsync(document);
            await _unitOfWork.SaveAsync(cancellationToken);  // now document.Id is persisted
            foreach (var cc in ccEntities)
            document.AddCc(cc);

            foreach (var tag in tagEntities)
            document.AddTag(tag);
            await _unitOfWork.SaveAsync(cancellationToken);


            // 6. History entry
            var history = new DocumentHistory(
                documentId: document.Id,
                profileId:  request.ProfileId,
                actionType: DocumentActions.Add,
                actionDate: DateTime.UtcNow,
                notes:      "تم انشاء المستند."
            );
            await _unitOfWork.Repository<DocumentHistory>().AddAsync(history);

            // 7. File uploads
            if (request.Files == null || !request.Files.Any())
                throw new ArgumentException("At least one file must be uploaded.");

// 7. رفع المرفقات وحفظها كمستند مرفق
foreach (var file in request.Files)
{
    // 7.a) استدعاء خدمة الرفع وتخزين النتيجة
    var uploadResult = await _photoService
        .AddPhotoAsync(file, document.Id, EntityType.Document);

    // 7.b) إنشاء المرفق باستخدام مسار الملف من uploadResult
    var docAttachment = new DocumentAttachment(
        filePath:   uploadResult.FilePath,
        documentId: document.Id
    );

    await _unitOfWork.Repository<DocumentAttachment>()
                     .AddAsync(docAttachment);
}

            // 8. Commit
            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new Exception("Failed to save document, attachments, or tags.");

            return document.Id;
        }
    }
}

// --- GetDocumentByDocumentNumberDetailedQueryHandler.cs ---
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentByDocumentNumberDetailedQueryHandler 
        : IRequestHandler<GetDocumentByDocumentNumberDetailedQuery, DocumentDetailedDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentByDocumentNumberDetailedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentDetailedDto> Handle(
            GetDocumentByDocumentNumberDetailedQuery request, 
            CancellationToken cancellationToken)
        {
            // 1) Eager‐load the single document + full hierarchy + links + child‐profiles
            var doc = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
.Include(d => d.Ministry)
    .Include(d => d.GeneralDirectorate)
    .Include(d => d.Directorate)
    .Include(d => d.Department)
    .Include(d => d.Section)

    // 2️⃣ If you want the full Section→…→Ministry chain:
    .Include(d => d.Section)
        .ThenInclude(sec => sec.Department)
            .ThenInclude(dep => dep.Directorate)
                .ThenInclude(dir => dir.GeneralDirectorate)
                    .ThenInclude(gd => gd.Ministry)

    // 3️⃣ And likewise for the department direct chain:
    .Include(d => d.Department)
        .ThenInclude(dep => dep.Directorate)
            .ThenInclude(dir => dir.GeneralDirectorate)
                .ThenInclude(gd => gd.Ministry)

    // 4️⃣ And for the directorate direct chain:
    .Include(d => d.Directorate)
        .ThenInclude(dir => dir.GeneralDirectorate)
            .ThenInclude(gd => gd.Ministry)

    // 5️⃣ And for the general directorate direct chain:
    .Include(d => d.GeneralDirectorate)
        .ThenInclude(gd => gd.Ministry)

    // … your other Includes (Project, PrivateParty, Profile, CCs, Tags, ChildDocuments)
    .Include(d => d.Project)
    .Include(d => d.PrivateParty)
    .Include(d => d.Profile)
    .Include(d => d.CcLinks).ThenInclude(l => l.DocumentCc)
    .Include(d => d.TagLinks).ThenInclude(l => l.Tag)
    .Include(d => d.ChildDocuments)
        .ThenInclude(cd => cd.CcLinks).ThenInclude(l => l.DocumentCc)
    .Include(d => d.ChildDocuments)
        .ThenInclude(cd => cd.TagLinks).ThenInclude(l => l.Tag)
    .Include(d => d.ChildDocuments)
        .ThenInclude(cd => cd.Profile)

                // ← NEW: load each child Document’s Profile
                // .Include(d => d.ChildDocuments)
                //     .ThenInclude(cd => cd.Profile)
                
                .FirstOrDefaultAsync(d => d.DocumentNumber == request.DocumentNumber, cancellationToken);

            if (doc == null)
                throw new KeyNotFoundException(
                    $"Document with number '{request.DocumentNumber}' not found.");

            // 2) Build and return the same recursive DTO tree
            return BuildDocumentTree(doc);
        }

        private DocumentDetailedDto BuildDocumentTree(Document d)
        {
            var dto = MapToDetailedDto(d);

            foreach (var child in d.ChildDocuments.OrderBy(cd => cd.DocumentDate))
                dto.ChildDocuments.Add(BuildDocumentTree(child));

            return dto;
        }

        private DocumentDetailedDto MapToDetailedDto(Document d)
        {
            // unwrap the chain
    var sec =   d.Section;
    var dep =   d.Department    ?? sec?.Department;
    var dir =   d.Directorate   ?? dep?.Directorate;
    var gd  =   d.GeneralDirectorate ?? dir?.GeneralDirectorate;
    var min =   d.Ministry      ?? gd?.Ministry;
            // safe CC/Tag names
            var ccNames = d.CcLinks
                .Where(l => l.DocumentCc != null)
                .Select(l => l.DocumentCc!.RecipientName ?? string.Empty)
                .ToList();
            var tagNames = d.TagLinks
                .Where(l => l.Tag != null)
                .Select(l => l.Tag!.Name ?? string.Empty)
                .ToList();

            return new DocumentDetailedDto
            {
                Id               = d.Id,
                DocumentNumber   = d.DocumentNumber,
                Title            = d.Title,
                DocumentType     = d.DocumentType,
                ResponseType     = d.ResponseType,
                Subject          = d.Subject,

                IsRequiresReply  = d.IsRequiresReply,
                IsReplied        = d.IsReplied,
                IsAudited        = d.IsAudited,
                IsUrgent         = d.IsUrgent,
                IsImportant      = d.IsImportant,
                IsNeeded         = d.IsNeeded,

                DocumentDate     = d.DocumentDate,
                Notes            = d.Notes,
                DateCreated      = d.DateCreated,

                ParentDocumentId = d.ParentDocumentId,
                ChildDocuments   = new List<DocumentDetailedDto>(),

                ProjectId        = d.ProjectId,
                ProjectName      = d.Project?.Name ?? string.Empty,

                // Section → Department → Directorate → GeneralDirectorate → Ministry
        SectionId              = sec?.Id,
        SectionName            = sec?.Name               ?? string.Empty,

        DepartmentId           = dep?.Id,
        DepartmentName         = dep?.Name              ?? string.Empty,

        DirectorateId          = dir?.Id,
        DirectorateName        = dir?.Name             ?? string.Empty,

        GeneralDirectorateId   = gd?.Id,
        GeneralDirectorateName = gd?.Name              ?? string.Empty,

        MinistryId             = min?.Id,
        MinistryName           = min?.Name             ?? string.Empty,
                PrivatePartyId           = d.PrivatePartyId,
                PrivatePartyName         = d.PrivateParty?.Name   ?? string.Empty,

                CcIds    = d.CcLinks.Select(l => l.DocumentCcId).ToList(),
                CcNames  = ccNames,
                TagIds   = d.TagLinks.Select(l => l.TagId).ToList(),
                TagNames = tagNames,

                ProfileId       = d.ProfileId,
                ProfileFullName = d.Profile?.FullName ?? string.Empty
            };
        }
    }
}

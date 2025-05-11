// Application/Queries/Documents/Handlers/GetDocumentsQueryHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Documents;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Documents.Handlers
{
    public class GetDocumentsQueryHandler 
        : IRequestHandler<GetDocumentsQuery, PagedList<DocumentDto>>
    {
        private readonly IGenericRepository<Document> _repository;

        public GetDocumentsQueryHandler(IGenericRepository<Document> repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<DocumentDto>> Handle(
            GetDocumentsQuery request,
            CancellationToken cancellationToken)
        {
            // 1) build spec
            var spec = new FilterDocumentsSpecification(
                documentNumber:   request.DocumentNumber,
                documentDate:     request.DocumentDate,
                title:            request.Title,
                subject:          request.Subject,
                documentType:     request.DocumentType,
                responseType:     request.ResponseType,
                isRequiresReply:  request.IsRequiresReply,
                isReplied:        request.IsReplied,
                isAudited:        request.IsAudited,
                isUrgent:         request.IsUrgent,
                isImportant:      request.IsImportant,
                isNeeded:         request.IsNeeded,
                notes:            request.Notes,
                projectId:        request.ProjectId,
                privatePartyId:   request.PrivatePartyId,
                parentDocumentId: request.ParentDocumentId,
                profileId:        request.ProfileId,
                sectionId:        request.SectionId,
                departmentId:     request.DepartmentId,
                directorateId:    request.DirectorateId,
                generalDirectorateId: request.GeneralDirectorateId,
                ministryId:       request.MinistryId
            );

            // 2) query with eager-load
            var query = _repository.ListAsQueryable(spec)
                .Include(d => d.Project)
                .Include(d => d.PrivateParty)
                .Include(d => d.Profile)
                .Include(d => d.CcLinks).ThenInclude(cl => cl.DocumentCc)
                .Include(d => d.TagLinks).ThenInclude(tl => tl.Tag)
                .Include(d => d.Section)
                            .Include(d => d.Ministry)
                                .Include(d => d.GeneralDirectorate)
                                    .Include(d => d.Directorate)
                                        .Include(d => d.Department)
                .OrderByDescending(d => d.DocumentDate);

            // 3) project into DocumentDto
var projected = query.Select(d => new DocumentDto
{
    Id                   = d.Id,
    DocumentNumber       = d.DocumentNumber,
    Title                = d.Title,
    DocumentType         = d.DocumentType,
    ResponseType         = d.ResponseType,
    Subject              = d.Subject,
    DocumentDate         = d.DocumentDate,

    IsRequiresReply      = d.IsRequiresReply,
    IsReplied            = d.IsReplied,
    IsAudited            = d.IsAudited,
    IsUrgent             = d.IsUrgent,
    IsImportant          = d.IsImportant,
    IsNeeded             = d.IsNeeded,

    Notes                = d.Notes,

    ProjectId            = d.ProjectId,
    PrivatePartyId       = d.PrivatePartyId,

    SectionId            = d.SectionId,
    SectionName          = d.Section != null
                               ? d.Section.Name
                               : string.Empty,

    DepartmentId         = d.Section != null
                               ? d.Section.DepartmentId
                               : (Guid?)null,
    DepartmentName       = d.Section != null && d.Section.Department != null
                               ? d.Section.Department.Name
                               : string.Empty,

    DirectorateId        = d.Section != null && d.Section.Department != null
                               ? d.Section.Department.DirectorateId
                               : (Guid?)null,
    DirectorateName      = d.Section != null && d.Section.Department != null 
                           && d.Section.Department.Directorate != null
                               ? d.Section.Department.Directorate.Name
                               : string.Empty,

    GeneralDirectorateId = d.Section != null && d.Section.Department != null
                           && d.Section.Department.Directorate != null
                               ? d.Section.Department.Directorate.GeneralDirectorateId
                               : (Guid?)null,
    GeneralDirectorateName = d.Section != null && d.Section.Department != null
                           && d.Section.Department.Directorate != null
                           && d.Section.Department.Directorate.GeneralDirectorate != null
                               ? d.Section.Department.Directorate.GeneralDirectorate.Name
                               : string.Empty,

    MinistryId           = d.Section != null && d.Section.Department != null
                           && d.Section.Department.Directorate != null
                           && d.Section.Department.Directorate.GeneralDirectorate != null
                               ? d.Section.Department.Directorate.GeneralDirectorate.MinistryId
                               : (Guid?)null,
    MinistryName         = d.Section != null && d.Section.Department != null
                           && d.Section.Department.Directorate != null
                           && d.Section.Department.Directorate.GeneralDirectorate != null
                           && d.Section.Department.Directorate.GeneralDirectorate.Ministry != null
                               ? d.Section.Department.Directorate.GeneralDirectorate.Ministry.Name
                               : string.Empty,

    ParentDocumentId     = d.ParentDocumentId,
    ProfileId            = d.ProfileId,

    ProjectName          = d.Project.Name,
    PrivatePartyName     = d.PrivateParty != null
                               ? d.PrivateParty.Name
                               : string.Empty,
    ProfileFullName      = d.Profile.FullName,

    CcIds                = d.CcLinks.Select(cl => cl.DocumentCcId).ToList(),
    CcNames              = d.CcLinks
                               .Where(cl => cl.DocumentCc != null)
                               .Select(cl => cl.DocumentCc.RecipientName!)
                               .ToList(),

    TagIds               = d.TagLinks.Select(tl => tl.TagId).ToList(),
    TagNames             = d.TagLinks
                               .Where(tl => tl.Tag != null)
                               .Select(tl => tl.Tag.Name!)
                               .ToList(),

    ChildDocuments       = new List<DocumentDto>(),
    DateCreated          = d.DateCreated
});


            // 4) paginate
            return await PagedList<DocumentDto>.CreateAsync(
                projected,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}

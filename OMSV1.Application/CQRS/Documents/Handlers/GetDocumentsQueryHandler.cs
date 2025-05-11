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
                
                // Always include any direct nav properties
                .Include(d => d.Ministry)
                .Include(d => d.GeneralDirectorate)
                .Include(d => d.Directorate)
                .Include(d => d.Department)
                .Include(d => d.Section)
                
                // Include the full Section→…→Ministry chain
                .Include(d => d.Section)
                    .ThenInclude(sec => sec.Department)
                        .ThenInclude(dep => dep.Directorate)
                            .ThenInclude(dir => dir.GeneralDirectorate)
                                .ThenInclude(gd => gd.Ministry)
                
                // Include Department→…→Ministry chain
                .Include(d => d.Department)
                    .ThenInclude(dep => dep.Directorate)
                        .ThenInclude(dir => dir.GeneralDirectorate)
                            .ThenInclude(gd => gd.Ministry)
                
                // Include Directorate→…→Ministry chain
                .Include(d => d.Directorate)
                    .ThenInclude(dir => dir.GeneralDirectorate)
                        .ThenInclude(gd => gd.Ministry)
                
                // Include GeneralDirectorate→Ministry chain
                .Include(d => d.GeneralDirectorate)
                    .ThenInclude(gd => gd.Ministry)
                
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

                // Use the same pattern as in GetDocumentByIdDetailedQueryHandler
                // handling both direct properties and hierarchical references
                SectionId            = d.SectionId,
                SectionName          = d.Section != null
                                         ? d.Section.Name
                                         : string.Empty,

                DepartmentId         = d.DepartmentId ?? (d.Section != null ? d.Section.DepartmentId : null),
                DepartmentName       = d.Department != null 
                                         ? d.Department.Name 
                                         : (d.Section != null && d.Section.Department != null 
                                            ? d.Section.Department.Name 
                                            : string.Empty),

                DirectorateId        = d.DirectorateId ?? 
                                      (d.Department != null ? d.Department.DirectorateId : 
                                       (d.Section != null && d.Section.Department != null 
                                        ? d.Section.Department.DirectorateId 
                                        : null)),
                DirectorateName      = d.Directorate != null 
                                         ? d.Directorate.Name 
                                         : (d.Department != null && d.Department.Directorate != null 
                                            ? d.Department.Directorate.Name 
                                            : (d.Section != null && d.Section.Department != null && d.Section.Department.Directorate != null 
                                               ? d.Section.Department.Directorate.Name 
                                               : string.Empty)),

                GeneralDirectorateId = d.GeneralDirectorateId ??
                                      (d.Directorate != null ? d.Directorate.GeneralDirectorateId :
                                       (d.Department != null && d.Department.Directorate != null 
                                        ? d.Department.Directorate.GeneralDirectorateId 
                                        : (d.Section != null && d.Section.Department != null && d.Section.Department.Directorate != null 
                                           ? d.Section.Department.Directorate.GeneralDirectorateId 
                                           : null))),
                GeneralDirectorateName = d.GeneralDirectorate != null 
                                           ? d.GeneralDirectorate.Name 
                                           : (d.Directorate != null && d.Directorate.GeneralDirectorate != null 
                                              ? d.Directorate.GeneralDirectorate.Name 
                                              : (d.Department != null && d.Department.Directorate != null && d.Department.Directorate.GeneralDirectorate != null 
                                                 ? d.Department.Directorate.GeneralDirectorate.Name 
                                                 : (d.Section != null && d.Section.Department != null && d.Section.Department.Directorate != null && d.Section.Department.Directorate.GeneralDirectorate != null 
                                                    ? d.Section.Department.Directorate.GeneralDirectorate.Name 
                                                    : string.Empty))),

                MinistryId           = d.MinistryId ??
                                      (d.GeneralDirectorate != null ? d.GeneralDirectorate.MinistryId :
                                       (d.Directorate != null && d.Directorate.GeneralDirectorate != null 
                                        ? d.Directorate.GeneralDirectorate.MinistryId 
                                        : (d.Department != null && d.Department.Directorate != null && d.Department.Directorate.GeneralDirectorate != null 
                                           ? d.Department.Directorate.GeneralDirectorate.MinistryId 
                                           : (d.Section != null && d.Section.Department != null && d.Section.Department.Directorate != null && d.Section.Department.Directorate.GeneralDirectorate != null 
                                              ? d.Section.Department.Directorate.GeneralDirectorate.MinistryId 
                                              : null)))),
                MinistryName         = d.Ministry != null 
                                         ? d.Ministry.Name 
                                         : (d.GeneralDirectorate != null && d.GeneralDirectorate.Ministry != null 
                                            ? d.GeneralDirectorate.Ministry.Name 
                                            : (d.Directorate != null && d.Directorate.GeneralDirectorate != null && d.Directorate.GeneralDirectorate.Ministry != null 
                                               ? d.Directorate.GeneralDirectorate.Ministry.Name 
                                               : (d.Department != null && d.Department.Directorate != null && d.Department.Directorate.GeneralDirectorate != null && d.Department.Directorate.GeneralDirectorate.Ministry != null 
                                                  ? d.Department.Directorate.GeneralDirectorate.Ministry.Name 
                                                  : (d.Section != null && d.Section.Department != null && d.Section.Department.Directorate != null && d.Section.Department.Directorate.GeneralDirectorate != null && d.Section.Department.Directorate.GeneralDirectorate.Ministry != null 
                                                     ? d.Section.Department.Directorate.GeneralDirectorate.Ministry.Name 
                                                     : string.Empty)))),

                ParentDocumentId     = d.ParentDocumentId,
                ProfileId            = d.ProfileId,

                ProjectName          = d.Project != null ? d.Project.Name : string.Empty,
                PrivatePartyName     = d.PrivateParty != null
                                           ? d.PrivateParty.Name
                                           : string.Empty,
                ProfileFullName      = d.Profile != null ? d.Profile.FullName : string.Empty,

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
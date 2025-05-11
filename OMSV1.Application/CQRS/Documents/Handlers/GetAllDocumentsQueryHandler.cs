// Application/Handlers/Documents/GetAllDocumentsQueryHandler.cs
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documents
{
    public class GetAllDocumentsQueryHandler 
        : IRequestHandler<GetAllDocumentsQuery, PagedList<DocumentDto>>
    {
        private readonly IGenericRepository<Document> _repository;
        private readonly IMapper                     _mapper;

        public GetAllDocumentsQueryHandler(
            IGenericRepository<Document> repository, 
            IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public async Task<PagedList<DocumentDto>> Handle(
            GetAllDocumentsQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                // 1) Base query + eager-load
                var query = _repository.GetAllAsQueryable()
                    .OrderByDescending(d => d.DocumentNumber)
                    .Include(d => d.Project)
                    .Include(d => d.PrivateParty)
                    .Include(d => d.Profile)
                        .Include(d => d.Department)
                            .Include(d => d.Ministry)
                                .Include(d => d.GeneralDirectorate)
                                    .Include(d => d.Directorate)
                                        .Include(d => d.Department)





                    .Include(d => d.CcLinks).ThenInclude(l => l.DocumentCc)
                    .Include(d => d.TagLinks).ThenInclude(l => l.Tag)
    // now load the whole tree under Section:
    .Include(d => d.Section);

                // 2) Manual projection to the updated DocumentDto
var projected = query.Select(d => new DocumentDto
{
    Id               = d.Id,
    DocumentNumber   = d.DocumentNumber,
    Title            = d.Title,
    DocumentType     = d.DocumentType,
    ResponseType     = d.ResponseType,
    Subject          = d.Subject,
    DocumentDate     = d.DocumentDate,

    IsRequiresReply  = d.IsRequiresReply,
    IsReplied        = d.IsReplied,
    IsAudited        = d.IsAudited,
    IsUrgent         = d.IsUrgent,
    IsImportant      = d.IsImportant,
    IsNeeded         = d.IsNeeded,

    Notes            = d.Notes,

    ProjectId        = d.ProjectId,
    PrivatePartyId   = d.PrivatePartyId,
    
    // Leaf
    SectionId        = d.SectionId,
    SectionName      = d.Section != null
                           ? d.Section.Name
                           : string.Empty,

    // Department
    DepartmentId     = d.Section != null
                           ? d.Section.DepartmentId
                           : (Guid?)null,
    DepartmentName   = d.Section != null && d.Section.Department != null
                           ? d.Section.Department.Name
                           : string.Empty,

    // Directorate
    DirectorateId    = d.Section != null && d.Section.Department != null
                           ? d.Section.Department.DirectorateId
                           : (Guid?)null,
    DirectorateName  = d.Section != null && d.Section.Department != null 
                           && d.Section.Department.Directorate != null
                           ? d.Section.Department.Directorate.Name
                           : string.Empty,

    // GeneralDirectorate
    GeneralDirectorateId   = d.Section != null && d.Section.Department != null
                           && d.Section.Department.Directorate != null
                           ? d.Section.Department.Directorate.GeneralDirectorateId
                           : (Guid?)null,
    GeneralDirectorateName = d.Section != null && d.Section.Department != null 
                           && d.Section.Department.Directorate != null
                           && d.Section.Department.Directorate.GeneralDirectorate != null
                           ? d.Section.Department.Directorate.GeneralDirectorate.Name
                           : string.Empty,

    // Ministry
    MinistryId       = d.Section != null && d.Section.Department != null
                           && d.Section.Department.Directorate != null
                           && d.Section.Department.Directorate.GeneralDirectorate != null
                           ? d.Section.Department.Directorate.GeneralDirectorate.MinistryId
                           : (Guid?)null,
    MinistryName     = d.Section != null && d.Section.Department != null
                           && d.Section.Department.Directorate != null
                           && d.Section.Department.Directorate.GeneralDirectorate != null
                           && d.Section.Department.Directorate.GeneralDirectorate.Ministry != null
                           ? d.Section.Department.Directorate.GeneralDirectorate.Ministry.Name
                           : string.Empty,

    ParentDocumentId = d.ParentDocumentId,
    ProfileId        = d.ProfileId,

    ProjectName      = d.Project.Name,
    PrivatePartyName = d.PrivateParty != null
                           ? d.PrivateParty.Name
                           : string.Empty,
    ProfileFullName  = d.Profile.FullName,

    CcIds   = d.CcLinks.Select(l => l.DocumentCcId).ToList(),
    CcNames = d.CcLinks
                 .Select(link => link.DocumentCc.RecipientName!)
                 .ToList(),
    TagIds  = d.TagLinks.Select(l => l.TagId).ToList(),
    TagNames = d.TagLinks
                  .Select(link => link.Tag.Name!)
                  .ToList(),

    DateCreated      = d.DateCreated
});


                // 3) Paginate (unchanged)
                var paged = await PagedList<DocumentDto>.CreateAsync(
                    projected,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return paged;
            }
            catch (Exception ex)
            {
                throw new HandlerException(
                    "An unexpected error occurred while retrieving all documents.",
                    ex
                );
            }
        }
    }
}

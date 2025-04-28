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
            // 1) Build your spec filter
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
                partyId:          request.PartyId,
                ministryId:       request.MinistryId,
                parentDocumentId: request.ParentDocumentId,
                profileId:        request.ProfileId
            );

            // 2) Start with the filtered queryable
            var query = _repository.ListAsQueryable(spec)
                .OrderByDescending(d => d.DocumentDate)

                // eager-load everything we need
                .Include(d => d.Project)
                .Include(d => d.Party)
                .Include(d => d.Ministry)
                .Include(d => d.Profile)
                .Include(d => d.CcLinks).ThenInclude(cl => cl.DocumentCc)
                .Include(d => d.TagLinks).ThenInclude(tl => tl.Tag);

            // 3) Manual projection into DocumentDto
            var projected = query.Select(d => new DocumentDto
            {
                Id                 = d.Id,
                DocumentNumber     = d.DocumentNumber,
                Title              = d.Title,
                DocumentType       = d.DocumentType,
                ResponseType       = d.ResponseType,
                Subject            = d.Subject,
                DocumentDate       = d.DocumentDate,

                IsRequiresReply    = d.IsRequiresReply,
                IsReplied          = d.IsReplied,
                IsAudited          = d.IsAudited,
                IsUrgent           = d.IsUrgent,
                IsImportant        = d.IsImportant,
                IsNeeded           = d.IsNeeded,

                Notes              = d.Notes,
                ParentDocumentId   = d.ParentDocumentId,

                ProjectId          = d.ProjectId,
                PartyId            = d.PartyId,
                MinistryId         = d.MinistryId,
                ProfileId          = d.ProfileId,

                // only names, no nested DTOs:
                ProjectName        = d.Project.Name,
                PartyName          = d.Party.Name,
                MinistryName       = d.Ministry != null 
                                        ? d.Ministry.Name 
                                        : string.Empty,
                ProfileFullName    = d.Profile.FullName,

           // flatten CC & tags to both names AND ids
    CcIds   = d.CcLinks.Select(cl => cl.DocumentCcId).ToList(),
    CcNames = d.CcLinks.Select(cl => cl.DocumentCc.RecipientName!)
                       .Where(n => n != null).ToList(),
    TagIds  = d.TagLinks.Select(tl => tl.TagId).ToList(),
    TagNames= d.TagLinks.Select(tl => tl.Tag.Name!)
                       .Where(n => n != null).ToList(),

    ChildDocuments = new List<DocumentDto>(),
    DateCreated    = d.DateCreated
            });

            // 4) Paginate exactly as before
            return await PagedList<DocumentDto>.CreateAsync(
                projected,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}

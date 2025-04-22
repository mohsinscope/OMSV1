using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        // 1) Base query: all documents flat list
        var query = _repository.GetAllAsQueryable()
            .OrderByDescending(d => d.DocumentNumber)

            // 2) Eager‑load related entities
            .Include(d => d.Profile)
            .Include(d => d.Party)
            .Include(d => d.Ministry)
            .Include(d => d.CcLinks)
                .ThenInclude(link => link.DocumentCc)
            .Include(d => d.TagLinks)
                .ThenInclude(link => link.Tag);

        // 3) Manual projection to DTO (no nested children)
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
            PartyId          = d.PartyId,
            MinistryId       = d.MinistryId,
            ParentDocumentId = d.ParentDocumentId,
            ProfileId        = d.ProfileId,

            Project          = null, // simplified; populate if needed
            Party            = null,
            Ministry         = null,
            Profile          = null,

            CcLinks          = d.CcLinks
                                .Select(l => new DocumentCCDto {
                                    Id = l.DocumentCcId,
                                    RecipientName = l.DocumentCc.RecipientName
                                })
                                .ToList(),
            TagLinks         = d.TagLinks
                                .Select(l => new TagsDto {
                                    Id = l.TagId,
                                    Name = l.Tag.Name
                                })
                                .ToList(),

            DateCreated      = d.DateCreated
        });

        // 4) Paginate
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
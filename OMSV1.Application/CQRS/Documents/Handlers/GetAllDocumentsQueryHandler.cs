// // Application/Handlers/Documents/GetAllDocumentsQueryHandler.cs
// using AutoMapper;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using OMSV1.Application.Dtos.Documents;
// using OMSV1.Application.Helpers;
// using OMSV1.Application.Queries.Documents;
// using OMSV1.Domain.Entities.Documents;
// using OMSV1.Domain.SeedWork;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;

// namespace OMSV1.Application.Handlers.Documents
// {
//     public class GetAllDocumentsQueryHandler 
//         : IRequestHandler<GetAllDocumentsQuery, PagedList<DocumentDto>>
//     {
//         private readonly IGenericRepository<Document> _repository;
//         private readonly IMapper                     _mapper;

//         public GetAllDocumentsQueryHandler(
//             IGenericRepository<Document> repository, 
//             IMapper mapper)
//         {
//             _repository = repository;
//             _mapper     = mapper;
//         }

//         public async Task<PagedList<DocumentDto>> Handle(
//             GetAllDocumentsQuery request, 
//             CancellationToken cancellationToken)
//         {
//             try
//             {
//                 // 1) Base query + eager-load
//                 var query = _repository.GetAllAsQueryable()
//                     .OrderByDescending(d => d.DocumentNumber)
//                     .Include(d => d.Project)
//                     .Include(d => d.Party)
//                     .Include(d => d.Ministry)
//                     .Include(d => d.Profile)
//                     .Include(d => d.CcLinks).ThenInclude(l => l.DocumentCc)
//                     .Include(d => d.TagLinks).ThenInclude(l => l.Tag);

//                 // 2) Manual projection to the updated DocumentDto
//                 var projected = query.Select(d => new DocumentDto
//                 {
//                     Id               = d.Id,
//                     DocumentNumber   = d.DocumentNumber,
//                     Title            = d.Title,
//                     DocumentType     = d.DocumentType,
//                     ResponseType     = d.ResponseType,
//                     Subject          = d.Subject,
//                     DocumentDate     = d.DocumentDate,

//                     IsRequiresReply  = d.IsRequiresReply,
//                     IsReplied        = d.IsReplied,
//                     IsAudited        = d.IsAudited,
//                     IsUrgent         = d.IsUrgent,
//                     IsImportant      = d.IsImportant,
//                     IsNeeded         = d.IsNeeded,

//                     Notes            = d.Notes,

//                     ProjectId        = d.ProjectId,
//                     PartyId          = d.PartyId,
//                     MinistryId       = d.MinistryId,
//                     ParentDocumentId = d.ParentDocumentId,
//                     ProfileId        = d.ProfileId,

//                     // only the names now
//                     ProjectName      = d.Project.Name,
//                     PartyName        = d.Party.Name,
//                     MinistryName     = d.Ministry != null ? d.Ministry.Name : string.Empty,
//                     ProfileFullName      = d.Profile.FullName,

//                     // flatten cc & tags to string lists
//                 CcNames          = d.CcLinks
//                                       .Select(link => link.DocumentCc.RecipientName)
//                                       .Where(name => name != null)
//                                       .ToList(),
//                 TagNames         = d.TagLinks
//                                       .Select(link => link.Tag.Name)
//                                       .Where(name => name != null)
//                                       .ToList(),

//                     DateCreated      = d.DateCreated
//                 });

//                 // 3) Paginate (unchanged)
//                 var paged = await PagedList<DocumentDto>.CreateAsync(
//                     projected,
//                     request.PaginationParams.PageNumber,
//                     request.PaginationParams.PageSize
//                 );

//                 return paged;
//             }
//             catch (Exception ex)
//             {
//                 throw new HandlerException(
//                     "An unexpected error occurred while retrieving all documents.",
//                     ex
//                 );
//             }
//         }
//     }
// }

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Documents;

namespace OMSV1.Application.Queries.Documents.Handlers
{
    public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, PagedList<DocumentDto>>
    {
        private readonly IGenericRepository<Document> _repository;
        private readonly IMapper _mapper;

        public GetDocumentsQueryHandler(IGenericRepository<Document> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DocumentDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
        {
            // Build the specification from the query parameters.
            var spec = new FilterDocumentsSpecification(
                documentNumber: request.DocumentNumber,
                documentDate: request.DocumentDate,
                title: request.Title,
                documentType: request.DocumentType,
                responseType: request.ResponseType,
                partyId: request.PartyId,
                isAudited: request.IsAudited,
                isReplied: request.IsReplied,
                isRequiresReply: request.IsRequiresReply
            );

            // Get an IQueryable<Document> filtered by the specification.
            var queryableResult = _repository.ListAsQueryable(spec);

            // Map to DocumentDto via AutoMapper.
            var mappedQuery = queryableResult.ProjectTo<DocumentDto>(_mapper.ConfigurationProvider);

            // Return a paginated list.
            return await PagedList<DocumentDto>.CreateAsync(mappedQuery, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
        }
    }
}

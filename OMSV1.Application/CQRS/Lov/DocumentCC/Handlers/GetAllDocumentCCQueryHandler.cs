using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DocumentCC;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Documentcc
{
    public class GetAllDocumentCCQueryHandler : IRequestHandler<GetAllDocumentCCQuery, PagedList<DocumentCCDto>>
    {
        private readonly IGenericRepository<DocumentCC> _repository;
        private readonly IMapper _mapper;

        public GetAllDocumentCCQueryHandler(IGenericRepository<DocumentCC> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DocumentCCDto>> Handle(GetAllDocumentCCQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all DocumentParty records as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project the queryable to DocumentPartyDto using AutoMapper
                var mappedQuery = queryable.ProjectTo<DocumentCCDto>(_mapper.ConfigurationProvider);

                // Create a paginated list using the provided pagination parameters
                var pagedDocumentParties = await PagedList<DocumentCCDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedDocumentParties;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all document parties.", ex);
            }
        }
    }
}

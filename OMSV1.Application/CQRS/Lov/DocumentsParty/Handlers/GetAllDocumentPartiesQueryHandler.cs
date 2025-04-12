using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DocumentParties;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.DocumentParties
{
    public class GetAllDocumentPartiesQueryHandler : IRequestHandler<GetAllDocumentPartiesQuery, PagedList<DocumentPartyDto>>
    {
        private readonly IGenericRepository<DocumentParty> _repository;
        private readonly IMapper _mapper;

        public GetAllDocumentPartiesQueryHandler(IGenericRepository<DocumentParty> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DocumentPartyDto>> Handle(GetAllDocumentPartiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all DocumentParty records as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project the queryable to DocumentPartyDto using AutoMapper
                var mappedQuery = queryable.ProjectTo<DocumentPartyDto>(_mapper.ConfigurationProvider);

                // Create a paginated list using the provided pagination parameters
                var pagedDocumentParties = await PagedList<DocumentPartyDto>.CreateAsync(
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

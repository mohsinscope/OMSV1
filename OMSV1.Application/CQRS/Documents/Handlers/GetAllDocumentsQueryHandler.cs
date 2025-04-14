using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documents
{
    public class GetAllDocumentsQueryHandler : IRequestHandler<GetAllDocumentsQuery, PagedList<DocumentDto>>
    {
        private readonly IGenericRepository<Document> _repository;
        private readonly IMapper _mapper;

        public GetAllDocumentsQueryHandler(IGenericRepository<Document> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DocumentDto>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all Document entities as an IQueryable.
            // Get all Document entities as an IQueryable.
            var queryable = _repository.GetAllAsQueryable();
                queryable = queryable.Where(d => d.ParentDocumentId == null)
                     .OrderByDescending(d => d.DocumentNumber);


                // Project the queryable to DocumentDto using AutoMapper.
                var mappedQuery = queryable.ProjectTo<DocumentDto>(_mapper.ConfigurationProvider);

                // Create and return a paginated list using the provided pagination parameters.
                var pagedDocuments = await PagedList<DocumentDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedDocuments;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all documents.", ex);
            }
        }
    }
}

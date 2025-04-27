// Application/Queries/Documents/Handlers/SearchByLinksQueryHandler.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Documents;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Documents.Handlers
{
    public class SearchByLinksQueryHandler 
        : IRequestHandler<SearchByLinksQuery, PagedList<DocumentDto>>
    {
        private readonly IGenericRepository<Document> _repository;
        private readonly IMapper                     _mapper;

        public SearchByLinksQueryHandler(
            IGenericRepository<Document> repository,
            IMapper mapper)              // <-- inject IMapper here
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public async Task<PagedList<DocumentDto>> Handle(
            SearchByLinksQuery request,
            CancellationToken cancellationToken)
        {
            // 1) Build spec
            var spec = new FilterDocumentsByLinksSpecification(
                ccIds:  request.CcIds,
                tagIds: request.TagIds);

            // 2) Apply spec and project via AutoMapper
            var queryable = _repository
                .ListAsQueryable(spec)
                .ProjectTo<DocumentDto>(_mapper.ConfigurationProvider);

            // 3) Paging unchanged
            return await PagedList<DocumentDto>.CreateAsync(
                queryable,
                request.PageNumber,
                request.PageSize);
        }
    }
}

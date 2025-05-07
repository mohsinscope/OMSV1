// Application/Queries/Documents/Handlers/SearchByLinksQueryHandler.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            IMapper mapper)
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

            // 2) Apply spec and EAGER-LOAD all needed navigations:
            var prepped = _repository.ListAsQueryable(spec)
                // core navigations
                .Include(d => d.Project)
                .Include(d => d.PrivateParty)
                .Include(d => d.Profile)

                // CC & Tag link details
                .Include(d => d.CcLinks)
                    .ThenInclude(cl => cl.DocumentCc)
                .Include(d => d.TagLinks)
                    .ThenInclude(tl => tl.Tag)

                // full Section→Department→Directorate→GeneralDirectorate→Ministry chain
                .Include(d => d.Section)
                    .ThenInclude(s => s.Department)
                        .ThenInclude(dep => dep.Directorate)
                            .ThenInclude(dir => dir.GeneralDirectorate)
                                .ThenInclude(gd => gd.Ministry);

            // 3) Project via AutoMapper into your new DocumentDto
            var projected = prepped
                .ProjectTo<DocumentDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(d => d.DocumentDate);

            // 4) Paging
            return await PagedList<DocumentDto>.CreateAsync(
                projected,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}

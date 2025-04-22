using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Tags;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Tags
{
    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, PagedList<TagsDto>>
    {
        private readonly IGenericRepository<Tag> _repository;
        private readonly IMapper _mapper;

        public GetAllTagsQueryHandler(IGenericRepository<Tag> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<TagsDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all TagsDto records as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project the queryable to TagsDto using AutoMapper
                var mappedQuery = queryable.ProjectTo<TagsDto>(_mapper.ConfigurationProvider);

                // Create a paginated list using the provided pagination parameters
                var pagedTags = await PagedList<TagsDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedTags;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all Tags.", ex);
            }
        }
    }
}

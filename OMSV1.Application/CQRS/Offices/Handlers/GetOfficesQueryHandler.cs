using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.CQRS.Offices.Queries;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Offices;

namespace OMSV1.Application.CQRS.Offices.Handlers
{
    public class GetOfficesQueryHandler : IRequestHandler<GetOfficesQuery, PagedList<OfficeDto>>
    {
        private readonly IGenericRepository<Office> _repository;
        private readonly IMapper _mapper;

        public GetOfficesQueryHandler(IGenericRepository<Office> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<OfficeDto>> Handle(GetOfficesQuery request, CancellationToken cancellationToken)
        {
            // Create a specification based on the query parameters.
            var spec = new FilterOfficesSpecification(
                request.GovernorateId,
                request.Name,
                request.IsEmbassy,
                request.IsTwoShifts,
                request.Code
            );

            // Retrieve a queryable collection of Office entities using the specification.
            var queryableResult = _repository.ListAsQueryable(spec);

            // Use AutoMapper to project Office entities to OfficeDto.
            var mappedQuery = queryableResult.ProjectTo<OfficeDto>(_mapper.ConfigurationProvider);

            // Return a paginated list.
            return await PagedList<OfficeDto>.CreateAsync(mappedQuery, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
        }
    }
}

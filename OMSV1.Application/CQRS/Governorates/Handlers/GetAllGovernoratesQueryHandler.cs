using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

public class GetAllGovernoratesQueryHandler : IRequestHandler<GetAllGovernoratesQuery, PagedList<GovernorateDto>>
{
    private readonly IGenericRepository<Governorate> _repository;
    private readonly IMapper _mapper;

    public GetAllGovernoratesQueryHandler(IGenericRepository<Governorate> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedList<GovernorateDto>> Handle(GetAllGovernoratesQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the governorates as IQueryable
        var governoratesQuery = _repository.GetAllAsQueryable();
        // Map to GovernorateDto using AutoMapper's ProjectTo
        var mappedQuery = governoratesQuery.ProjectTo<GovernorateDto>(_mapper.ConfigurationProvider);

        // Apply pagination using PagedList
        var pagedGovernorates = await PagedList<GovernorateDto>.CreateAsync(
            mappedQuery,
            request.PaginationParams.PageNumber,
            request.PaginationParams.PageSize
        );

        return pagedGovernorates;
    }
}

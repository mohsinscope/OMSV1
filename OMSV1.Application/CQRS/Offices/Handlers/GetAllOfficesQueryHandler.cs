using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Offices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

public class GetAllOfficesQueryHandler : IRequestHandler<GetAllOfficesQuery, PagedList<OfficeDto>>
{
    private readonly IGenericRepository<Office> _repository;
    private readonly IMapper _mapper;

    public GetAllOfficesQueryHandler(IGenericRepository<Office> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedList<OfficeDto>> Handle(GetAllOfficesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the offices as IQueryable
            var officesQuery = _repository.GetAllAsQueryable();

            // Sort by governorate
            var sortedQuery = officesQuery.OrderBy(o => o.Governorate);

            // Map to OfficeDto using AutoMapper's ProjectTo
            var mappedQuery = sortedQuery.ProjectTo<OfficeDto>(_mapper.ConfigurationProvider);

            // Apply pagination
            var pagedOffices = await PagedList<OfficeDto>.CreateAsync(
                mappedQuery,
                request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize
            );

            return pagedOffices;
        }
        catch (Exception ex)
        {
            // Log and throw a custom exception if an error occurs
            throw new HandlerException("An error occurred while retrieving the offices.", ex);
        }
    }
}

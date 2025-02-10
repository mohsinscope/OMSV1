using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Application.Helpers; // For HandlerException and PagedList
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.CQRS.Lov.DamagedPassport
{
public class GetAllDamagedTypesQueryHandler : IRequestHandler<GetAllDamagedTypesQuery, PagedList<DamagedTypeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllDamagedTypesQueryHandler> _logger;

    public GetAllDamagedTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllDamagedTypesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<DamagedTypeDto>> Handle(GetAllDamagedTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve damaged types as IQueryable
            var damagedTypesQuery = _unitOfWork.Repository<DamagedType>().GetAllAsQueryable();

            // Check if the query is null
            if (damagedTypesQuery == null)
            {
                _logger.LogError("The GetAllAsQueryable returned null for DamagedType.");
                throw new NullReferenceException("DamagedType query returned null.");
            }

            // Optionally order the results
            damagedTypesQuery = damagedTypesQuery.OrderBy(dt => dt.Name);

            // Project the query to DamagedTypeDto using AutoMapper
            var mappedQuery = damagedTypesQuery.ProjectTo<DamagedTypeDto>(_mapper.ConfigurationProvider);

            // Create paginated list
            var pagedDamagedTypes = await PagedList<DamagedTypeDto>.CreateAsync(
                mappedQuery,
                request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize
            );

            return pagedDamagedTypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving damaged types.");
            throw new HandlerException("An error occurred while retrieving damaged types.", ex);
        }
    }
}

}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;

public class GetAllAttendancesQueryHandler : IRequestHandler<GetAllAttendancesQuery, PagedList<AttendanceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAttendancesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<AttendanceDto>> Handle(GetAllAttendancesQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Attendance>().GetAllAsQueryable();
                    // Apply ordering here - replace 'Date' with the field you want to order by
            query = query.OrderByDescending(dp => dp.Date);  // Example: Order by Date in descending order

        var mappedQuery = query.ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider);

        return await PagedList<AttendanceDto>.CreateAsync(
            mappedQuery,
            request.PaginationParams.PageNumber,
            request.PaginationParams.PageSize
        );
    }
}

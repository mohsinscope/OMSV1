using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

public class GetAllAttendancesQueryHandler : IRequestHandler<GetAllAttendancesQuery, PagedList<AttendanceAllDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAttendancesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<AttendanceAllDto>> Handle(GetAllAttendancesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Attendance>().GetAllAsQueryable();

            // Apply ordering - order by Date in descending order (you can replace 'Date' with any field)
            query = query.OrderByDescending(dp => dp.Date);

            // Project the results to AttendanceAllDto using AutoMapper
            var mappedQuery = query.ProjectTo<AttendanceAllDto>(_mapper.ConfigurationProvider);

            // Create the paginated list of AttendanceAllDto
            return await PagedList<AttendanceAllDto>.CreateAsync(
                mappedQuery,
                request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize
            );
        }
        catch (Exception ex)
        {
            // Optionally log the error here if necessary
            throw new HandlerException("An error occurred while fetching attendance records.", ex);
        }
    }
}

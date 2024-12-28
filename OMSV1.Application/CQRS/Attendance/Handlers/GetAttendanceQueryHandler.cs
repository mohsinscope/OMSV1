using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Helpers;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Attendances;

namespace OMSV1.Application.CQRS.Attendances.Handlers
{
    public class GetAttendanceQueryHandler : IRequestHandler<GetAttendanceQuery, PagedList<AttendanceDto>>
    {
        private readonly IGenericRepository<OMSV1.Domain.Entities.Attendances.Attendance> _repository;
        private readonly IMapper _mapper;

        public GetAttendanceQueryHandler(IGenericRepository<OMSV1.Domain.Entities.Attendances.Attendance> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<AttendanceDto>> Handle(GetAttendanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Create the specification based on the query parameters
                var spec = new FilterAttendanceSpecification(
                    request.WorkingHours, 
                    request.StartDate,    
                    request.EndDate,      
                    request.OfficeId,     
                    request.GovernorateId, 
                    request.ProfileId);

                // Get the queryable list of Attendance entities
                var queryableResult = _repository.ListAsQueryable(spec);

                // Map to AttendanceDto
                var mappedQuery = queryableResult.ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider);

                // Create a paginated list of AttendanceDto
                return await PagedList<AttendanceDto>.CreateAsync(mappedQuery, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
            }
            catch (Exception ex)
            {
                // Optionally log the error here if necessary
                throw new HandlerException("An error occurred while fetching the attendance records.", ex);
            }
        }
    }
}

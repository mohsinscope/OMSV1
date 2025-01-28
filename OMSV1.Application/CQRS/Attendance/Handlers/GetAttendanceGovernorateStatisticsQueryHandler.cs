using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Specifications.Attendances;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Attendances
{
    public class GetAttendanceGovernorateStatisticsQueryHandler : IRequestHandler<GetAttendanceGovernorateStatisticsQuery, AttendanceGovernorateStatisticsResponseDto>
    {
        private readonly IGenericRepository<Attendance> _repository;
        private readonly IMapper _mapper;

        public GetAttendanceGovernorateStatisticsQueryHandler(IGenericRepository<Attendance> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AttendanceGovernorateStatisticsResponseDto> Handle(GetAttendanceGovernorateStatisticsQuery request, CancellationToken cancellationToken)
        {
            // Convert the date to UTC
            var filterDateUtc = request.Date.ToUniversalTime();

            // Apply filtering using the specified date in UTC
            var filterSpec = new FilterAttendanceSpecification(
                startDate: filterDateUtc,
                endDate: filterDateUtc.AddDays(1).AddTicks(-1)); // Filter only for the given day

            var attendances = await _repository.ListAsync(filterSpec);

            // Group by governorate and calculate statistics
            var groupedData = attendances
                .GroupBy(a => a.Governorate.Name)
                .Select(g => new AttendanceGovernorateStatisticsDto
                {
                    GovernorateName = g.Key,
                    MorningShiftCount = g
                        .Where(a => a.WorkingHours.HasFlag(WorkingHours.Morning))
                        .Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff),
                    EveningShiftCount = g
                        .Where(a => a.WorkingHours.HasFlag(WorkingHours.Evening))
                        .Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff)
                })
                .ToList();

            return new AttendanceGovernorateStatisticsResponseDto
            {
                GovernorateStatistics = groupedData
            };
        }
    }
}

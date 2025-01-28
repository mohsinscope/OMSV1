using MediatR;
using OMSV1.Application.Dtos.Attendances;

namespace OMSV1.Application.Queries.Attendances
{
    public class GetAttendanceGovernorateStatisticsQuery : IRequest<AttendanceGovernorateStatisticsResponseDto>
    {
        public DateTime Date { get; set; } // The specified date for filtering attendance
    }
}

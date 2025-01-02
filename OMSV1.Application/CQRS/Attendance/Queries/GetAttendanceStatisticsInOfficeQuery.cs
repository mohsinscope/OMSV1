using MediatR;
using OMSV1.Application.Dtos.Attendances;

namespace OMSV1.Application.CQRS.Attendance.Queries
{
    public class GetAttendanceStatisticsInOfficeQuery : IRequest<AttendanceStatisticsInOfficeDto>
    {
        public int? OfficeId { get; set; }
        public int? WorkingHours { get; set; }
        public DateTime? Date { get; set; }

        public GetAttendanceStatisticsInOfficeQuery(int? officeId, int? workingHours, DateTime? date)
        {
            OfficeId = officeId;
            WorkingHours = workingHours;
            Date = date;
        }
    }
}

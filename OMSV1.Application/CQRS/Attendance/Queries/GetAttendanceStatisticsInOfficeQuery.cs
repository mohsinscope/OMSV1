using MediatR;
using OMSV1.Application.Dtos.Attendances;

namespace OMSV1.Application.CQRS.Attendance.Queries
{
    public class GetAttendanceStatisticsInOfficeQuery : IRequest<AttendanceStatisticsInOfficeDto>
    {
        public Guid? OfficeId { get; set; }
        public int? WorkingHours { get; set; }
        public DateTime? Date { get; set; }

        public GetAttendanceStatisticsInOfficeQuery(Guid? officeId, int? workingHours, DateTime? date)
        {
            OfficeId = officeId;
            WorkingHours = workingHours;
            Date = date;
        }
    }
}

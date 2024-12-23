using MediatR;
using OMSV1.Application.Dtos.Attendances;

namespace OMSV1.Application.Queries.Attendances
{
    public class SearchAttendanceStatisticsQuery : IRequest<AttendanceStatisticsDto>
    {
        public int? OfficeId { get; set; }
        public string? StaffType { get; set; } // Optional: Filter by staff type (e.g., ReceivingStaff)
        public DateTime? Date { get; set; }    // Filter by a specific date
        public int? GovernorateId { get; set; } // Filter by governorate
        public int? WorkingHours { get; set; } // Filter by working hours (enum)
    }
}

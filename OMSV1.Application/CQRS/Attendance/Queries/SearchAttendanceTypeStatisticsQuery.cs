using MediatR;
using OMSV1.Application.Dtos.Attendances;
using System;

namespace OMSV1.Application.Queries.Attendances
{
    public class SearchAttendanceTypeStatisticsQuery : IRequest<List<AttendanceTypeStatisticsDto>>
    {
        public Guid? OfficeId { get; set; }
        public string? StaffType { get; set; } // Required: Filter by staff type (e.g., ReceivingStaff)
        public DateTime? Date { get; set; }    // Required: Filter by a specific date
        public Guid? GovernorateId { get; set; } // Optional: Filter by governorate
        public int? WorkingHours { get; set; } // Optional: Filter by working hours (enum)
    }
}

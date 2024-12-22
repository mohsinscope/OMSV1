using System;
using MediatR;
using OMSV1.Application.Dtos.Attendance;

namespace OMSV1.Application.CQRS.Attendance.Commands;

public class GetAttendanceStatisticsQuery : IRequest<AttendanceStatisticsDto>
{
    public int? GovernorateId { get; set; }
    public int? OfficeId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

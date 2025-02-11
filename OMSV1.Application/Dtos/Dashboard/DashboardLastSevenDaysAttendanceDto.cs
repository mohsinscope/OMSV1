using System;
using System.Collections.Generic;

namespace OMSV1.Application.Dtos.Dashboard
{
    /// <summary>
    /// DTO that holds attendance percentages for each of the last 7 days.
    /// </summary>
    public class DashboardLastSevenDaysAttendanceDto
    {
        public List<DailyAttendanceDto> DailyAttendance { get; set; } = new List<DailyAttendanceDto>();
    }

    /// <summary>
    /// DTO representing the attendance percentage for a single day.
    /// </summary>
    public class DailyAttendanceDto
    {
        /// <summary>
        /// The date for which this attendance percentage is computed.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The attendance percentage (rounded to 2 decimal places) for that day.
        /// </summary>
        public decimal AttendancePercentage { get; set; }
    }
}

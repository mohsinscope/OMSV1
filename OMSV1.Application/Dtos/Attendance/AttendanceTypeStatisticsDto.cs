namespace OMSV1.Application.Dtos.Attendances
{
    public class AttendanceTypeStatisticsDto
    {        public string OfficeName { get; set; }  // Office Name
        public int AvailableStaff { get; set; } // Available staff from attendance
        public int TotalStaff { get; set; }     // Total staff from office
        public string StaffType { get; set; }   // The staff type being queried
    }
}
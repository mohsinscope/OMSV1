namespace OMSV1.Application.Dtos.Attendances
{
    public class AttendanceStatisticsDto
    {
        public int TotalStaffCount { get; set; }
        public int TotalStaffInOffice { get; set; }
        public int AvailableStaff { get; set; }
        public int AvailableStaffInOffice { get; set; }
        public int AvailableSpecificStaff { get; set; }

        // New fields
        public int TotalSpecificStaff { get; set; } // Total staff of the specified type across all offices
        public int TotalSpecificStaffInOffice { get; set; } // Total staff of the specified type in the specified office
        
        // Percentages
        public double AvailableStaffPercentage { get; set; } // Percentage of available staff across all offices
        public double AvailableStaffInOfficePercentage { get; set; } // Percentage of available staff in the specified office
        public double AvailableSpecificStaffPercentage { get; set; } // Percentage of available specific staff
    }
}

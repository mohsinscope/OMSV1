namespace OMSV1.Application.Dtos.Attendances
{
    public class AttendanceGovernorateStatisticsDto
    {
        public required string GovernorateName { get; set; } // Governorate name
        public int MorningShiftCount { get; set; } // Count of staff in Morning shift
        public int EveningShiftCount { get; set; } // Count of staff in Evening shift
    }

    public class AttendanceGovernorateStatisticsResponseDto
    {
        public List<AttendanceGovernorateStatisticsDto> GovernorateStatistics { get; set; } = new();
    }
}

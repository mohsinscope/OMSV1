    namespace OMSV1.Application.Dtos.Dashboard;
    public class DashboardStatisticsDto
    {
        // 1. Total number of offices
        public int TotalOffices { get; set; }
        
        // 2. Total number of governorates
        public int TotalGovernorates { get; set; }
        
        // 3. Total number of staff (sum of all staff types) in all offices
        public int TotalStaffInAllOffices { get; set; }
        
        // 4. Total number of each staff in all offices
        public int TotalReceivingStaff { get; set; }
        public int TotalAccountStaff { get; set; }
        public int TotalPrintingStaff { get; set; }
        public int TotalQualityStaff { get; set; }
        public int TotalDeliveryStaff { get; set; }
        
        // 5. Total number of damaged passports registered today
        public int TotalDamagedPassportsToday { get; set; }
        
        // 6. Percentage of attendances (attended vs. expected staff) for today, across all offices.
public decimal AttendancePercentageMorning { get; set; }
public decimal AttendancePercentageEvening { get; set; }
    }
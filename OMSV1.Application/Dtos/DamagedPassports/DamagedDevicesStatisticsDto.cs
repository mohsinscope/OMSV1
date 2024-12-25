using System;

namespace OMSV1.Application.Dtos
{
    public class DamagedPassportsStatisticsDto
    {
        // Available Damaged Passports across all offices and governorates
        public int AvailableDamagedPassports { get; set; }
        
        // Available Damaged Passports in a specific office
        public int AvailableDamagedPassportsInOffice { get; set; }
        
        // Available Damaged Passports of a specific type across all offices
        public int AvailableSpecificDamagedPassports { get; set; }

        // Available Damaged Passports of a specific type in a specific office
        public int AvailableSpecificDamagedPassportsInOffice { get; set; }
    }
}

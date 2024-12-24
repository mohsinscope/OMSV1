using System;

namespace OMSV1.Application.Dtos.DamagedDevices
{
    public class DamagedDevicesStatisticsDto
    {
        // Available Damaged Devices across all offices and governorates
        public int AvailableDamagedDevices { get; set; }
        
        // Available Damaged Devices in a specific office
        public int AvailableDamagedDevicesInOffice { get; set; }
        
        // Available Damaged Devices of a specific type across all offices
        public int AvailableSpecificDamagedDevices { get; set; }

        // Available Damaged Devices of a specific type in a specific office
        public int AvailableSpecificDamagedDevicesInOffice { get; set; }
    }
}

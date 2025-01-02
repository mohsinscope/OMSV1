using MediatR;
using OMSV1.Application.Dtos.DamagedDevices;

namespace OMSV1.Application.Queries.DamagedDevices
{
    public class SearchDamagedDevicesStatisticsQuery : IRequest<DamagedDevicesStatisticsDto>
    {
        public Guid? OfficeId { get; set; }            // Filter by specific office (optional)
        public Guid? GovernorateId { get; set; }       // Filter by governorate (optional)
        public DateTime? StartDate { get; set; } // Start of the date range
        public DateTime? EndDate { get; set; }   // End of the date range        
        public Guid? DamagedDeviceTypeId { get; set; } // Filter by a specific damaged device type (optional)
    }
}

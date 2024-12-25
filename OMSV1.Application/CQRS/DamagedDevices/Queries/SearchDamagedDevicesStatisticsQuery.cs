using MediatR;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Dtos.DamagedDevices;

namespace OMSV1.Application.Queries.DamagedDevices
{
    public class SearchDamagedDevicesStatisticsQuery : IRequest<DamagedDevicesStatisticsDto>
    {
        public int? OfficeId { get; set; }            // Filter by specific office (optional)
        public int? GovernorateId { get; set; }       // Filter by governorate (optional)
        public DateTime? Date { get; set; }           // Filter by a specific date (optional)
        public int? DamagedDeviceTypeId { get; set; } // Filter by a specific damaged device type (optional)
    }
}

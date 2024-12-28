using MediatR;
using OMSV1.Application.Dtos;

namespace OMSV1.Application.Queries.DamagedPassports
{
    public class SearchDamagedPassportsStatisticsQuery : IRequest<DamagedPassportsStatisticsDto>
    {
        public int? OfficeId { get; set; }            // Filter by specific office (optional)
        public int? GovernorateId { get; set; }       // Filter by governorate (optional)
        public DateTime? Date { get; set; }           // Filter by a specific date (optional)
        public int? DamagedTypeId { get; set; } // Filter by a specific damaged device type (optional)
    }
}

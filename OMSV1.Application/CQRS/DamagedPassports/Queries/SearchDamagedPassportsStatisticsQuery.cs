using MediatR;
using OMSV1.Application.Dtos;

namespace OMSV1.Application.Queries.DamagedPassports
{
    public class SearchDamagedPassportsStatisticsQuery : IRequest<DamagedPassportsStatisticsDto>
    {
        public Guid? OfficeId { get; set; }            // Filter by specific office (optional)
        public Guid? GovernorateId { get; set; }       // Filter by governorate (optional)
        public DateTime? Date { get; set; }           // Filter by a specific date (optional)
        public Guid? DamagedTypeId { get; set; } // Filter by a specific damaged device type (optional)
    }
}

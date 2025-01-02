using OMSV1.Application.Dtos.Offices;

namespace OMSV1.Application.Dtos.Governorates
{
    public class GovernorateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public List<OfficeDto> Offices { get; set; } = new();
    }
}

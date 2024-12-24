using OMSV1.Application.Dtos.Offices;

namespace OMSV1.Application.Dtos.Governorates
{
    public class GovernorateWithOfficesDropdownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OfficeDropdownDto> Offices { get; set; }

    }
}

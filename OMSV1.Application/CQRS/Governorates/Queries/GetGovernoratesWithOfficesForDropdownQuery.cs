using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Dtos.Offices;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernoratesWithOfficesForDropdownQuery : IRequest<List<GovernorateWithOfficesDropdownDto>>
    {
        public int? GovernorateId { get; set; } // Make it nullable

        public GetGovernoratesWithOfficesForDropdownQuery(int? governorateId = null)
        {
            GovernorateId = governorateId;
        }
    }
}

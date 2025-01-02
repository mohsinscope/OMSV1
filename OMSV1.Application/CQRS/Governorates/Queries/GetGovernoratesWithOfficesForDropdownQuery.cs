using MediatR;
using OMSV1.Application.Dtos.Governorates;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernoratesWithOfficesForDropdownQuery : IRequest<List<GovernorateWithOfficesDropdownDto>>
    {
        public Guid? GovernorateId { get; set; } // Make it nullable

        public GetGovernoratesWithOfficesForDropdownQuery(Guid? governorateId = null)
        {
            GovernorateId = governorateId;
        }
    }
}

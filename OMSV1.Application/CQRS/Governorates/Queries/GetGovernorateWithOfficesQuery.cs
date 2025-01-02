using MediatR;
using OMSV1.Application.Dtos.Governorates;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernorateWithOfficesQuery : IRequest<GovernorateWithOfficesDto>
    {
        public Guid Id { get; }

        public GetGovernorateWithOfficesQuery(Guid id)
        {
            Id = id;
        }
    }
}

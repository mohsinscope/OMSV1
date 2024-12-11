using MediatR;
using OMSV1.Application.Dtos.Governorates;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernorateWithOfficesQuery : IRequest<GovernorateWithOfficesDto>
    {
        public int Id { get; }

        public GetGovernorateWithOfficesQuery(int id)
        {
            Id = id;
        }
    }
}

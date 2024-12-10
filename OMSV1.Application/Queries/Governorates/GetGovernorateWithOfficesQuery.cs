using MediatR;
using OMSV1.Domain.Entities.Governorates;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernorateWithOfficesQuery : IRequest<Governorate>
    {
        public int Id { get; }

        public GetGovernorateWithOfficesQuery(int id)
        {
            Id = id;
        }
    }
}

using MediatR;
using OMSV1.Domain.Entities.Governorates;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernorateByIdQuery : IRequest<Governorate>
    {
        public int Id { get; }

        public GetGovernorateByIdQuery(int id)
        {
            Id = id;
        }
    }
}

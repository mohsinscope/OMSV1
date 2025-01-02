using MediatR;
using OMSV1.Application.Dtos.Governorates;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernorateByIdQuery : IRequest<GovernorateDto>
    {
        public Guid Id { get; }

        public GetGovernorateByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
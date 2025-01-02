using MediatR;
using OMSV1.Application.Dtos.LOV;

namespace OMSV1.Application.CQRS.Lov.DamagedPassport
{
    public class GetDamagedTypeByIdQuery : IRequest<DamagedTypeDto>
    {
        public Guid Id { get; set; }

        public GetDamagedTypeByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

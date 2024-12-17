using MediatR;
using OMSV1.Application.Dtos.LOV;

namespace OMSV1.Application.CQRS.Lov.DamagedPassport
{
    public class GetAllDamagedTypesQuery : IRequest<List<DamagedTypeDto>>
    {
    }
}

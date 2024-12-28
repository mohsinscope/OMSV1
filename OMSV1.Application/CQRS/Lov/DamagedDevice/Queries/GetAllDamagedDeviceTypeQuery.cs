using MediatR;
using OMSV1.Application.Dtos;

namespace OMSV1.Application.Commands.LOV
{
    public class GetAllDamagedDeviceTypesQuery : IRequest<List<DamagedDeviceTypeDto>>
    {
    }
}

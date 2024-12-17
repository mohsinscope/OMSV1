using MediatR;
using OMSV1.Application.Dtos.LOV;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class GetAllDeviceTypesQuery : IRequest<IEnumerable<DeviceTypeDto>> 
    {
    }
}

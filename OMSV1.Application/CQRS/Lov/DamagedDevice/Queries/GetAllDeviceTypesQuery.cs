using MediatR;
using OMSV1.Application.Dtos.LOV;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    // Change IEnumerable to List to align with IRequestHandler expectation
    public class GetAllDeviceTypesQuery : IRequest<List<DeviceTypeDto>> 
    {
    }
}

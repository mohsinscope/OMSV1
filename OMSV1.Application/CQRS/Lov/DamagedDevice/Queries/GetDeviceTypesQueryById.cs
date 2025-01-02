using MediatR;
using OMSV1.Application.Dtos.LOV;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class GetDeviceTypesQueryById : IRequest<DeviceTypeDto> 
    {
        public Guid Id { get; set; }

        public GetDeviceTypesQueryById(Guid id)
        {
            Id = id;
        }
    }
}

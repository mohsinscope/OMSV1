using MediatR;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class AddDeviceTypeCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

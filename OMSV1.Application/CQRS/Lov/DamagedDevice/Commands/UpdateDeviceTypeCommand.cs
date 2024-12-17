using MediatR;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class UpdateDeviceTypeCommand : IRequest<bool> // The command returns a boolean indicating success/failure
    {
        public int Id { get; set; } // The ID of the device type to update
        public string Name { get; set; } // New name for the device type
        public string Description { get; set; } // New description for the device type
    }
}

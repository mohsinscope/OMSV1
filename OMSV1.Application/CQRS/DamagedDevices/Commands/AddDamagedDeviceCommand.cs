using MediatR;

namespace OMSV1.Application.Commands.DamagedDevices
{
    public class AddDamagedDeviceCommand : IRequest<Guid>
    {
        public string?  SerialNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid DamagedDeviceTypeId { get; set; }
        public Guid DeviceTypeId { get; set; }
        public string? Note { get; set; } = ""; // Default value for Note if not provided

        public Guid OfficeId { get; set; }
        public Guid GovernorateId { get; set; }
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }  // Add this property to pass the UserId

    }
}

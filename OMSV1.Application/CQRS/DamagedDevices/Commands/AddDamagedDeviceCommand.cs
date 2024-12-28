using MediatR;

namespace OMSV1.Application.Commands.DamagedDevices
{
    public class AddDamagedDeviceCommand : IRequest<int>
    {
        public string?  SerialNumber { get; set; }
        public DateTime Date { get; set; }
        public int DamagedDeviceTypeId { get; set; }
        public int DeviceTypeId { get; set; }
        public string Note { get; set; } = ""; // Default value for Note if not provided

        public int OfficeId { get; set; }
        public int GovernorateId { get; set; }
        public int ProfileId { get; set; }
        public int UserId { get; set; }  // Add this property to pass the UserId

    }
}

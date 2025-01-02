using MediatR;
using System;

namespace OMSV1.Application.Commands.DamagedDevices
{
    public class UpdateDamagedDeviceCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid DamagedDeviceTypeId { get; set; }
        public Guid DeviceTypeId { get; set; }
        public string? Note { get; set; }
        public Guid OfficeId { get; set; }
        public Guid GovernorateId { get; set; }
        public Guid ProfileId { get; set; }
    }
}

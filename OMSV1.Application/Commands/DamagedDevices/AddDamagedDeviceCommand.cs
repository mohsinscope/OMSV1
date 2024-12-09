using MediatR;
using System;

namespace OMSV1.Application.Commands.DamagedDevices
{
    public class AddDamagedDeviceCommand : IRequest<int>
    {
        public string SerialNumber { get; set; }
        public DateTime Date { get; set; }
        public int DamagedDeviceTypeId { get; set; }
        public int DeviceTypeId { get; set; }
        public int OfficeId { get; set; }
        public int GovernorateId { get; set; }
        public int ProfileId { get; set; }
    }
}

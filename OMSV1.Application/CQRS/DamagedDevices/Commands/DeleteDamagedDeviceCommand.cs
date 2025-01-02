using MediatR;

namespace OMSV1.Application.Commands.DamagedDevices
{
    public class DeleteDamagedDeviceCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteDamagedDeviceCommand(Guid id)
        {
            Id = id;
        }
    }
}

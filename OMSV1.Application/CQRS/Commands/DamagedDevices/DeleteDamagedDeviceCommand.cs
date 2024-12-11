using MediatR;

namespace OMSV1.Application.Commands.DamagedDevices
{
    public class DeleteDamagedDeviceCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteDamagedDeviceCommand(int id)
        {
            Id = id;
        }
    }
}

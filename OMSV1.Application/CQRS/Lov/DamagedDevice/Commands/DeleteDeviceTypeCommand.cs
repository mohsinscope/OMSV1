using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDeviceTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }

        // You can pass the ID when creating the command
        public DeleteDeviceTypeCommand(int id)
        {
            Id = id;
        }
    }
}

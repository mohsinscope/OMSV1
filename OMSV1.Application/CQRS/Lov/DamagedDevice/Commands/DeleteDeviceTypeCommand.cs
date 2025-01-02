using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDeviceTypeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        // You can pass the ID when creating the command
        public DeleteDeviceTypeCommand(Guid id)
        {
            Id = id;
        }
    }
}

using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDamagedTypeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        // You can pass the ID when creating the command
        public DeleteDamagedTypeCommand(Guid id)
        {
            Id = id;
        }
    }
}

using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDamagedTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }

        // You can pass the ID when creating the command
        public DeleteDamagedTypeCommand(int id)
        {
            Id = id;
        }
    }
}

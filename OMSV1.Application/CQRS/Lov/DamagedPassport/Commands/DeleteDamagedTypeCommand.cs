using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDamagedTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

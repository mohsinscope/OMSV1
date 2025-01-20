using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class AddDamagedTypeCommand : IRequest<bool>  // Returns true if successful
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}

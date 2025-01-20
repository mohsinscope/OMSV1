using MediatR;

namespace OMSV1.Application.CQRS.Lov.DamagedPassport
{
    public class UpdateDamagedTypeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}

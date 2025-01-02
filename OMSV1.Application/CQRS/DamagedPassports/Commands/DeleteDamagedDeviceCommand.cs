using MediatR;

namespace OMSV1.Application.Commands.DamagedPassports
{
    public class DeleteDamagedPassportCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteDamagedPassportCommand(Guid id)
        {
            Id = id;
        }
    }
}

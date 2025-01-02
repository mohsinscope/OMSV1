using MediatR;

namespace OMSV1.Application.Commands.Attendances
{
    public class DeleteAttendanceCommand : IRequest<Unit>
    {
        public Guid Id { get; }

        public DeleteAttendanceCommand(Guid id)
        {
            Id = id;
        }
    }
}

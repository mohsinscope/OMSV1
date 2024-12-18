using MediatR;

namespace OMSV1.Application.Commands.Attendances
{
    public class DeleteAttendanceCommand : IRequest<Unit>
    {
        public int Id { get; }

        public DeleteAttendanceCommand(int id)
        {
            Id = id;
        }
    }
}

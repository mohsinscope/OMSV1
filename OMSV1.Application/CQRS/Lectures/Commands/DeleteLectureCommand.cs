using MediatR;

namespace OMSV1.Application.Commands.Lectures
{
    public class DeleteLectureCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteLectureCommand(Guid id)
        {
            Id = id;
        }
    }
}

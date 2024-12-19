using MediatR;

namespace OMSV1.Application.Commands.Lectures
{
    public class DeleteLectureCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteLectureCommand(int id)
        {
            Id = id;
        }
    }
}

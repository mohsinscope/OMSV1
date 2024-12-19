using MediatR;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Lectures
{
    public class DeleteLectureCommandHandler : IRequestHandler<DeleteLectureCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLectureCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteLectureCommand request, CancellationToken cancellationToken)
        {
            // Fetch the lecture entity
            var lecture = await _unitOfWork.Repository<Lecture>().GetByIdAsync(request.Id);

            if (lecture == null)
                return false; // If not found, return false

            // Perform the delete operation
            await _unitOfWork.Repository<Lecture>().DeleteAsync(lecture);

            // Save the changes to the database
            if (await _unitOfWork.SaveAsync(cancellationToken))
            {
                return true; // Successfully deleted
            }

            return false; // Failed to save the changes
        }
    }
}

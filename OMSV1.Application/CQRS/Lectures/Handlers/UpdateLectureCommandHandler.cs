using MediatR;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Lectures
{
    public class UpdateLectureCommandHandler : IRequestHandler<UpdateLectureCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLectureCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateLectureCommand request, CancellationToken cancellationToken)
        {
            // Get the lecture using the repository inside unit of work
            var lecture = await _unitOfWork.Repository<Lecture>().GetByIdAsync(request.Id);

            if (lecture == null) return false; // Lecture not found

            // Update the lecture details
            lecture.UpdateLectureDetails(
                request.Title,
                request.Date,
                request.Note,
                request.OfficeId,
                request.GovernorateId,
                request.ProfileId
            );

            // Update the entity using the repository inside unit of work
            await _unitOfWork.Repository<Lecture>().UpdateAsync(lecture);

            // Save the changes using unit of work
            if (await _unitOfWork.SaveAsync(cancellationToken))
            {
                return true;
            }

            return false;
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Application.Helpers;
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
            try
            {
                // Get the lecture
                var lecture = await _unitOfWork.Repository<Lecture>().GetByIdAsync(request.Id);
                if (lecture == null) return false; // Return false if not found

                // Update lecture details
                lecture.UpdateLectureDetails(
                    request.Title,
                    request.Date,
                    request.Note ?? string.Empty, // Default to empty if null
                    request.OfficeId,
                    request.GovernorateId,
                    request.ProfileId,
                    request.CompanyId,
                    new List<Guid>() // Replace null with an empty list
                );

                // Remove existing LectureLectureTypes
                var existingLectureTypes = await _unitOfWork.Repository<LectureLectureType>()
                    .GetAllAsQueryable()
                    .Where(llt => llt.LectureId == request.Id)
                    .ToListAsync(cancellationToken);

                foreach (var existing in existingLectureTypes)
                {
                    await _unitOfWork.Repository<LectureLectureType>().DeleteAsync(existing);
                }

                // Add the new LectureTypes
                foreach (var lectureTypeId in request.LectureTypeIds)
                {
                    var lectureLectureType = new LectureLectureType(request.Id, lectureTypeId);
                    await _unitOfWork.Repository<LectureLectureType>().AddAsync(lectureLectureType);
                }

                // Update the lecture entity in the repository
                await _unitOfWork.Repository<Lecture>().UpdateAsync(lecture);

                // Save changes to the database
                return await _unitOfWork.SaveAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while updating the lecture.", ex);
            }
        }
    }
}

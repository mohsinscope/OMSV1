using MediatR;
using OMSV1.Application.Commands.LectureTypes;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.LectureTypes
{
    public class UpdateLectureTypeCommandHandler : IRequestHandler<UpdateLectureTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLectureTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateLectureTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the lecture type entity by its ID
                var lectureType = await _unitOfWork.Repository<LectureType>().GetByIdAsync(request.LectureTypeId);

                if (lectureType == null)
                {
                    throw new HandlerException($"Lecture type with ID {request.LectureTypeId} not found.");
                }

                // Update the lecture type's name using the UpdateName method
                lectureType.UpdateName(request.Name);

                // Save the changes to the database
                if (await _unitOfWork.SaveAsync(cancellationToken))
                {
                    return true; // Successfully updated
                }

                return false; // Failed to save the changes
            }
            catch (HandlerException ex)
            {
                // Log and rethrow the custom exception
                throw new HandlerException("Error occurred while updating the lecture type.", ex);
            }
            catch (Exception ex)
            {
                // Catch unexpected errors and rethrow them as HandlerException
                throw new HandlerException("An unexpected error occurred while updating the lecture type.", ex);
            }
        }
    }
}

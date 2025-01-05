using MediatR;
using OMSV1.Application.Commands.LectureTypes;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.LectureTypes
{
    public class DeleteLectureTypeCommandHandler : IRequestHandler<DeleteLectureTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLectureTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteLectureTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the lecture type entity by its ID
                var lectureType = await _unitOfWork.Repository<LectureType>().GetByIdAsync(request.LectureTypeId);

                if (lectureType == null)
                {
                    throw new HandlerException($"Lecture type with ID {request.LectureTypeId} not found.");
                }

                // Remove the lecture type entity
                await _unitOfWork.Repository<LectureType>().DeleteAsync(lectureType);

                // Save the changes to the database
                if (await _unitOfWork.SaveAsync(cancellationToken))
                {
                    return true; // Successfully deleted
                }

                return false; // Failed to save the changes
            }
            catch (HandlerException ex)
            {
                // Log and rethrow the custom exception
                throw new HandlerException("Error occurred while deleting the lecture type.", ex);
            }
            catch (Exception ex)
            {
                // Catch unexpected errors and rethrow them as HandlerException
                throw new HandlerException("An unexpected error occurred while deleting the lecture type.", ex);
            }
        }
    }
}

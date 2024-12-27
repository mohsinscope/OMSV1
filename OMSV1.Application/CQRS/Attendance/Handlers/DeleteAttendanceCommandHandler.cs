using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Attendances
{
    public class DeleteAttendanceCommandHandler : IRequestHandler<DeleteAttendanceCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAttendanceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Fetch the attendance from the repository
                var attendance = await _unitOfWork.Repository<Attendance>().GetByIdAsync(request.Id);

                // Step 2: If attendance doesn't exist, throw a custom exception
                if (attendance == null)
                {
                    throw new HandlerException($"Attendance with ID {request.Id} not found.");
                }

                // Step 3: Proceed with deleting the attendance
                await _unitOfWork.Repository<Attendance>().DeleteAsync(attendance);
                await _unitOfWork.SaveAsync(cancellationToken);

                // Step 4: Return success (no content)
                return Unit.Value;
            }
            catch (HandlerException ex)
            {
                // Catch the specific HandlerException and rethrow it
                throw new HandlerException("Error occurred while deleting attendance.", ex);
            }
            catch (Exception ex)
            {
                // Catch unexpected errors and rethrow them as HandlerException
                throw new HandlerException("An unexpected error occurred while deleting attendance.", ex);
            }
        }
    }
}

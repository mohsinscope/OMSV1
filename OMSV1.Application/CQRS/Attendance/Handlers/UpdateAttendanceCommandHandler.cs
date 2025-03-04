using MediatR;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Handlers.Attendances
{
    public class UpdateAttendanceCommandHandler : IRequestHandler<UpdateAttendanceCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAttendanceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the existing attendance record
                var attendance = await _unitOfWork.Repository<Attendance>().GetByIdAsync(request.Id);

                if (attendance == null)
                    throw new KeyNotFoundException($"Attendance with ID {request.Id} not found.");

                // Convert the int value for WorkingHours to the enum
                var workingHoursEnum = (WorkingHours)request.WorkingHours;

                // Check if another attendance record already exists with WorkingHours.Morning
                if (workingHoursEnum == WorkingHours.Morning)
                {
                    var existingMorningAttendance = await _unitOfWork.Repository<Attendance>()
                        .FirstOrDefaultAsync(a =>
                            a.Date == request.Date &&
                            a.GovernorateId == request.GovernorateId &&
                            a.OfficeId == request.OfficeId &&
                            a.ProfileId == request.ProfileId &&
                            a.WorkingHours == WorkingHours.Morning &&
                            a.Id != request.Id); // Ensure it's not the same record

                    if (existingMorningAttendance != null)
                        throw new HandlerException("An attendance record with 'Morning' working hours already exists for this date, office, and profile.");
                }

                // Map the updated data from the request to the attendance entity
                attendance.UpdateDetails(
                    request.ReceivingStaff,
                    request.AccountStaff,
                    request.PrintingStaff,
                    request.QualityStaff,
                    request.DeliveryStaff,
                    request.Date,
                    request.Note ?? string.Empty, // Default to an empty string if null
                    workingHoursEnum, // Now passing the enum
                    request.GovernorateId,
                    request.OfficeId,
                    request.ProfileId
                );

                // Update the entity in the repository
                await _unitOfWork.Repository<Attendance>().UpdateAsync(attendance);

                // Save changes to the database
                await _unitOfWork.SaveAsync(cancellationToken);

                // Return confirmation (Unit.Value represents a successful operation)
                return Unit.Value;
            }
            catch (KeyNotFoundException knfEx)
            {
                // Handle not found error
                throw new HandlerException("Attendance record not found.", knfEx);
            }
            catch (Exception ex)
            {
                // Log and rethrow as a HandlerException for consistent error handling
                throw new HandlerException("An error occurred while updating the attendance record.", ex);
            }
        }
    }
}

using MediatR;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;

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
            // Retrieve the existing attendance record
            var attendance = await _unitOfWork.Repository<Attendance>().GetByIdAsync(request.Id);

            if (attendance == null)
                throw new KeyNotFoundException($"Attendance with ID {request.Id} not found.");

            // Convert the int value for WorkingHours to the enum
            var workingHoursEnum = (WorkingHours)request.WorkingHours;

            // Map the updated data from the request to the attendance entity
            attendance.UpdateDetails(
                request.ReceivingStaff,
                request.AccountStaff,
                request.PrintingStaff,
                request.QualityStaff,
                request.DeliveryStaff,
                request.Date,
                request.Note,
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
    }
}

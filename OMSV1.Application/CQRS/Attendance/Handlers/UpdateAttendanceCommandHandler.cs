using MediatR;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;

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
        var attendance = await _unitOfWork.Repository<Attendance>().GetByIdAsync(request.Id);

        if (attendance == null)
            throw new KeyNotFoundException($"Attendance with ID {request.Id} not found.");

        attendance.UpdateDetails(
            request.ReceivingStaff,
            request.AccountStaff,
            request.PrintingStaff,
            request.QualityStaff,
            request.DeliveryStaff,
            request.Date,
            request.Note,
            request.WorkingHours,
            request.GovernorateId,
            request.OfficeId,
            request.ProfileId
        );

        await _unitOfWork.Repository<Attendance>().UpdateAsync(attendance);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}

}

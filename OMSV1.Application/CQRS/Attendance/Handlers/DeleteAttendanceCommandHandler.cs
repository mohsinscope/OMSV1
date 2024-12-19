using MediatR;
using OMSV1.Application.Commands.Attendances;
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
            var attendance = await _unitOfWork.Repository<Attendance>().GetByIdAsync(request.Id);

            if (attendance == null)
                throw new KeyNotFoundException($"Attendance with ID {request.Id} not found.");

            await _unitOfWork.Repository<Attendance>().DeleteAsync(attendance);
            await _unitOfWork.SaveAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

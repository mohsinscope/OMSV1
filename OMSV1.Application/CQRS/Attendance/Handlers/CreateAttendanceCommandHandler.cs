using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Attendances; // Ensure the correct namespace

namespace OMSV1.Application.Handlers.Attendances
{
    public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAttendanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate that the Office belongs to the provided Governorate.
                var office = await _unitOfWork.Repository<Office>()
                    .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);
                if (office == null)
                {
                    throw new HandlerException($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
                }

                // Ensure the Date is set to the current date if not provided.
                if (request.Date == default)
                {
                    request.Date = DateTime.UtcNow;
                }

                // Normalize the date by defining a range for the day.
                var attendanceDate = request.Date.Date;
                var nextDate = attendanceDate.AddDays(1);

                // Create a specification to retrieve all attendances for the office on the given day.
                var attendanceSpec = new AttendanceByOfficeAndDateSpecification(request.OfficeId, attendanceDate, nextDate);
                
                // Remove the cancellationToken parameter since ListAsync only takes a single specification argument.
                var attendancesForDay = await _unitOfWork.Repository<Attendance>().ListAsync(attendanceSpec);

                // Check for Morning conflict.
                if (((int)request.WorkingHours & (int)WorkingHours.Morning) == (int)WorkingHours.Morning)
                {
                    bool hasMorningConflict = attendancesForDay.Any(a =>
                        (((int)a.WorkingHours & (int)WorkingHours.Morning) == (int)WorkingHours.Morning));
                    if (hasMorningConflict)
                    {
                        throw new HandlerException($"An attendance for the Morning shift on {attendanceDate:d} already exists for office ID {request.OfficeId}.");
                    }
                }

                // Check for Evening conflict.
                if (((int)request.WorkingHours & (int)WorkingHours.Evening) == (int)WorkingHours.Evening)
                {
                    bool hasEveningConflict = attendancesForDay.Any(a =>
                        (((int)a.WorkingHours & (int)WorkingHours.Evening) == (int)WorkingHours.Evening));
                    if (hasEveningConflict)
                    {
                        throw new HandlerException($"An attendance for the Evening shift on {attendanceDate:d} already exists for office ID {request.OfficeId}.");
                    }
                }

                // Map the command to the Attendance entity.
                var attendance = _mapper.Map<Attendance>(request);

                // Update the attendance date to ensure it is in UTC.
                attendance.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

                // Add the attendance entity to the repository.
                await _unitOfWork.Repository<Attendance>().AddAsync(attendance);

                // Save changes to the database.
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new HandlerException("Failed to save the Attendance to the database.");
                }

                // Return the ID of the newly created attendance.
                return attendance.Id;
            }
            catch (HandlerException ex)
            {
                throw new HandlerException("An error occurred while processing the attendance creation request.", ex);
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred.", ex);
            }
        }
    }
}

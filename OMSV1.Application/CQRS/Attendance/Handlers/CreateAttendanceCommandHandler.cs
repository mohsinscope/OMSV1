using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.Handlers.Attendances;
public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAttendanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
    {
        // Ensure the Date is set to current date if not provided
        if (request.Date == default)
        {
            request.Date = DateTime.UtcNow;  // Use current date if not specified
        }

        // Ensure WorkingHours has a default value if not set
        if (request.WorkingHours <= 0)
        {
            request.WorkingHours = 3;  // Default working hours if not specified
        }

        // Map the command to the entity
        var attendance = _mapper.Map<Attendance>(request);

        // Convert Date to UTC if needed
        attendance.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

        // Save the entity using UnitOfWork
        await _unitOfWork.Repository<Attendance>().AddAsync(attendance);

        if (!await _unitOfWork.SaveAsync(cancellationToken))
        {
            throw new Exception("Failed to save the Attendance to the database.");
        }

        return attendance.Id;
    }
}

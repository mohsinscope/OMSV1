using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;

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
    // Map the command to the entity
    var attendance = _mapper.Map<Attendance>(request);

    // Convert Date to UTC before saving
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
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.Handlers.Attendances
{
    public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        // Constructor to inject dependencies
        public CreateAttendanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Validate if the OfficeId belongs to the GovernorateId using FirstOrDefaultAsync
            var office = await _unitOfWork.Repository<Office>()
                .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);

            if (office == null)
            {
                // If the office doesn't belong to the governorate, throw an exception
                throw new Exception($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
            }

            // Step 2: Ensure the Date is set to current date if not provided
            if (request.Date == default)
            {
                request.Date = DateTime.UtcNow;  // Use current date if not specified
            }

            // Step 4: Map the command to the entity
            var attendance = _mapper.Map<Attendance>(request);

            // Step 5: Convert Date to UTC if needed
            attendance.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

            // Step 6: Add the attendance entity to the repository
            await _unitOfWork.Repository<Attendance>().AddAsync(attendance);

            // Step 7: Save changes to the database
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to save the Attendance to the database.");
            }

            // Step 8: Return the ID of the newly created attendance
            return attendance.Id;
        }
    }
}

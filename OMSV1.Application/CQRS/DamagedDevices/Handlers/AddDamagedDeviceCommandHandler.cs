using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.Queries.Profiles;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class AddDamagedDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator, AppDbContext context) : IRequestHandler<AddDamagedDeviceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly AppDbContext _context;

        public async Task<int> Handle(AddDamagedDeviceCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Validate if the OfficeId belongs to the GovernorateId using FirstOrDefaultAsync
            var office = await _unitOfWork.Repository<Office>()
                .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);

            if (office == null)
            {
                // If the office doesn't belong to the governorate, throw an exception
                throw new Exception($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
            }

            // Step 2: Map the request to the DamagedDevice entity
            var damagedDevice = _mapper.Map<DamagedDevice>(request);

            // Step 3: Convert Date to UTC before saving
            damagedDevice.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

            // Step 4: Add the damaged device to the repository using AddAsync
            await _unitOfWork.Repository<DamagedDevice>().AddAsync(damagedDevice);

            // Step 5: Save changes to the database
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                // Handle failure to save
                throw new Exception("Failed to save the damaged device to the database.");
            }

            // Step 6: Return the ID of the newly created damaged device
            return damagedDevice.Id;
        }


    }
}

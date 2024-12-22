using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.Queries.Profiles;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class AddDamagedDeviceCommandHandler : IRequestHandler<AddDamagedDeviceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public AddDamagedDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,IMediator mediator, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<int> Handle(AddDamagedDeviceCommand request, CancellationToken cancellationToken)
        {


            var damagedDevice = _mapper.Map<DamagedDevice>(request);

             // Convert Date to UTC before saving
             damagedDevice.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));


            await _unitOfWork.Repository<DamagedDevice>().AddAsync(damagedDevice);

            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to save the damaged device to the database.");
            }

            return damagedDevice.Id;
        }

    }
}

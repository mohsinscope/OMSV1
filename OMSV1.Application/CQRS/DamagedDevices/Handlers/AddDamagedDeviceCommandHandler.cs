using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class AddDamagedDeviceCommandHandler : IRequestHandler<AddDamagedDeviceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDamagedDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddDamagedDeviceCommand request, CancellationToken cancellationToken)
{
    var damagedDevice = _mapper.Map<DamagedDevice>(request);

    // Convert UTC to local (remove Kind)
damagedDevice.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Unspecified));


    await _unitOfWork.Repository<DamagedDevice>().AddAsync(damagedDevice);

    if (!await _unitOfWork.SaveAsync(cancellationToken))
    {
        throw new Exception("Failed to save the damaged device to the database.");
    }

    return damagedDevice.Id;
}

    }
}

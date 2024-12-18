using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.Queries.Profiles;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class AddDamagedDeviceCommandHandler : IRequestHandler<AddDamagedDeviceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AddDamagedDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<int> Handle(AddDamagedDeviceCommand request, CancellationToken cancellationToken)
{
    // Assuming you pass the UserId in the command
    var profileId = await _mediator.Send(new GetProfileIdByUserIdQuery(request.UserName));
    request.ProfileId = profileId;

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

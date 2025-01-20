using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.LOV
{
    public class GetDamagedDeviceTypeQueryHandler : IRequestHandler<GetDamagedDeviceTypeQuery, DamagedDeviceTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDamagedDeviceTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

public async Task<DamagedDeviceTypeDto> Handle(GetDamagedDeviceTypeQuery request, CancellationToken cancellationToken)
{
    try
    {
        // Fetch the damaged device type by ID
        var damagedDeviceType = await _unitOfWork.Repository<DamagedDeviceType>().GetByIdAsync(request.Id);

        if (damagedDeviceType == null)
        {
            throw new KeyNotFoundException($"DamagedDeviceType with ID {request.Id} not found.");
        }

        // Map and return the DTO
        return new DamagedDeviceTypeDto
        {
            Id = damagedDeviceType.Id,
            Name = damagedDeviceType.Name,
            Description = damagedDeviceType.Description
        };
    }
    catch (Exception ex)
    {
        throw new HandlerException("An error occurred while retrieving the damaged device type.", ex);
    }
}

    }
}

using MediatR;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.Dtos.LOV;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class GetAllDeviceTypesQueryHandler : IRequestHandler<GetAllDeviceTypesQuery, List<DeviceTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDeviceTypesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DeviceTypeDto>> Handle(GetAllDeviceTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch all device types from the repository
                var deviceTypes = await _unitOfWork.Repository<DeviceType>().GetAllAsync();

                // Map the entities to DTOs
                var deviceTypesDto = deviceTypes.Select(dt => new DeviceTypeDto
                {
                    Id = dt.Id,
                    Name = dt.Name,
                    Description = dt.Description
                }).ToList();

                return deviceTypesDto;
            }
            catch (Exception ex)
            {
                // Log and throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while fetching all device types.", ex);
            }
        }
    }
}

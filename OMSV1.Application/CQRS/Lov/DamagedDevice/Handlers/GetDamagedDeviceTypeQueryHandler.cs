using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Application.Helpers;
using System.Threading;
using System.Threading.Tasks;
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
                // Fetch the damaged device type by ID using the unit of work and repository
                var damagedDeviceType = await _unitOfWork.Repository<DamagedDeviceType>().GetByIdAsync(request.Id);

                if (damagedDeviceType == null)
                    return null; // Return null if the device type is not found

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
                // If an error occurs, throw a custom HandlerException with the error message and inner exception
                throw new HandlerException("An error occurred while retrieving the damaged device type.", ex);
            }
        }
    }
}

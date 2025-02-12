using MediatR;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Application.Helpers; // For HandlerException and PagedList
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using System.Linq;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    // Note: Return type has been changed to PagedList<DeviceTypeDto>
    public class GetAllDeviceTypesQueryHandler : IRequestHandler<GetAllDeviceTypesQuery, PagedList<DeviceTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDeviceTypesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<DeviceTypeDto>> Handle(GetAllDeviceTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the DeviceTypes as an IQueryable
                var deviceTypesQuery = _unitOfWork.Repository<DeviceType>().GetAllAsQueryable();
                
                if (deviceTypesQuery == null)
                {
                    throw new NullReferenceException("DeviceType query returned null.");
                }

                // Order the results (adjust the ordering field as needed)
                deviceTypesQuery = deviceTypesQuery.OrderBy(dt => dt.Name);

                // Project the query to DeviceTypeDto
                var mappedQuery = deviceTypesQuery.Select(dt => new DeviceTypeDto
                {
                    Id = dt.Id,
                    Name = dt.Name,
                    Description = dt.Description
                });

                // Create a paginated list using the provided pagination parameters
                var pagedDeviceTypes = await PagedList<DeviceTypeDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedDeviceTypes;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while fetching all device types.", ex);
            }
        }
    }
}

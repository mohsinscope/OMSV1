using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedDevices;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OMSV1.Application.Commands.LOV
{
    // Update the return type to PagedList<DamagedDeviceTypeDto>
    public class GetAllDamagedDeviceTypesQueryHandler : IRequestHandler<GetAllDamagedDeviceTypesQuery, PagedList<DamagedDeviceTypeDto>>
    {
        private readonly IGenericRepository<DamagedDeviceType> _damagedDeviceTypeRepository;

        public GetAllDamagedDeviceTypesQueryHandler(IGenericRepository<DamagedDeviceType> damagedDeviceTypeRepository)
        {
            _damagedDeviceTypeRepository = damagedDeviceTypeRepository;
        }

        public async Task<PagedList<DamagedDeviceTypeDto>> Handle(GetAllDamagedDeviceTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the damaged device types as an IQueryable
                var queryable = _damagedDeviceTypeRepository.GetAllAsQueryable();
                if (queryable == null)
                {
                    throw new NullReferenceException("DamagedDeviceType query returned null.");
                }

                // Order the results (adjust the ordering as needed)
                queryable = queryable.OrderBy(ddt => ddt.Name);

                // Project the query to DamagedDeviceTypeDto
                var projectedQuery = queryable.Select(ddt => new DamagedDeviceTypeDto
                {
                    Id = ddt.Id,
                    Name = ddt.Name,
                    Description = ddt.Description
                });

                // Create a paginated list using the provided pagination parameters
                var paginatedList = await PagedList<DamagedDeviceTypeDto>.CreateAsync(
                    projectedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return paginatedList;
            }
            catch (Exception ex)
            {
                // Throw a custom exception to be handled by your middleware or higher-level exception handler
                throw new HandlerException("An error occurred while retrieving the damaged device types.", ex);
            }
        }
    }
}

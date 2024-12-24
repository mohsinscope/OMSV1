using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.DamagedDevices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.DamagedDevices
{
    public class SearchDamagedDevicesStatisticsHandler : IRequestHandler<SearchDamagedDevicesStatisticsQuery, DamagedDevicesStatisticsDto>
    {
        private readonly IGenericRepository<DamagedDevice> _damagedDeviceRepo;
        private readonly IGenericRepository<Office> _officeRepo;

        public SearchDamagedDevicesStatisticsHandler(
            IGenericRepository<DamagedDevice> damagedDeviceRepo,
            IGenericRepository<Office> officeRepo)
        {
            _damagedDeviceRepo = damagedDeviceRepo;
            _officeRepo = officeRepo;
        }

        public async Task<DamagedDevicesStatisticsDto> Handle(SearchDamagedDevicesStatisticsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<DamagedDevice> damagedDeviceQuery;

            // Determine which query to execute based on the filters provided
            if (!request.GovernorateId.HasValue)
            {
                // Fetch all damaged devices in all offices and governorates
                damagedDeviceQuery = _damagedDeviceRepo.ListAsQueryable(new FilterDamagedDevicesStatisticsSpecification(
                    officeId: request.OfficeId,
                    damagedDeviceTypeId: request.DamagedDeviceTypeId,
                    date: request.Date
                ));
            }
            else if (request.OfficeId.HasValue)
            {
                // Fetch damaged devices filtered by both governorate and office
                damagedDeviceQuery = _damagedDeviceRepo.ListAsQueryable(new FilterDamagedDevicesStatisticsSpecification(
                    officeId: request.OfficeId,
                    governorateId: request.GovernorateId,
                    damagedDeviceTypeId: request.DamagedDeviceTypeId,
                    date: request.Date
                ));
            }
            else
            {
                // Fetch damaged devices filtered by governorate (for all offices in that governorate)
                damagedDeviceQuery = _damagedDeviceRepo.ListAsQueryable(new FilterDamagedDevicesStatisticsSpecification(
                    governorateId: request.GovernorateId,
                    damagedDeviceTypeId: request.DamagedDeviceTypeId,
                    date: request.Date
                ));
            }

            // Aggregate the data by office
            var totalDamagedDevices = await damagedDeviceQuery
                .GroupBy(d => d.OfficeId)
                .Select(group => new
                {
                    OfficeId = group.Key,
                    // Directly compare dates without time truncation
                    AvailableDamagedDevices = group.Count(d => d.Date.Date == request.Date.Value.Date), 
                    AvailableSpecificDamagedDevices = group.Count(d => d.DamagedDeviceTypeId == request.DamagedDeviceTypeId && d.Date.Date == request.Date.Value.Date)
                })
                .ToListAsync(cancellationToken);

            // Fetch office data (filtered by GovernorateId if necessary)
            var offices = await _officeRepo.GetAllAsQueryable()
                .Where(o => !request.GovernorateId.HasValue || o.GovernorateId == request.GovernorateId)
                .Select(o => new
                {
                    o.Id,
                    o.Name
                })
                .ToListAsync(cancellationToken);

            // Calculate total and available damaged devices across all offices
            var availableDamagedDevicesCount = totalDamagedDevices.Sum(d => d.AvailableDamagedDevices);
            var availableSpecificDamagedDevicesCount = totalDamagedDevices.Sum(d => d.AvailableSpecificDamagedDevices);

            // If a specific office is provided, calculate the totals for that office
            var officeSpecificTotal = totalDamagedDevices.Where(d => d.OfficeId == request.OfficeId).Sum(d => d.AvailableDamagedDevices);
            var officeSpecificAvailable = totalDamagedDevices.Where(d => d.OfficeId == request.OfficeId).Sum(d => d.AvailableSpecificDamagedDevices);

            return new DamagedDevicesStatisticsDto
            {
                AvailableDamagedDevices = availableDamagedDevicesCount,
                AvailableDamagedDevicesInOffice = officeSpecificTotal,
                AvailableSpecificDamagedDevices = availableSpecificDamagedDevicesCount,
                AvailableSpecificDamagedDevicesInOffice = officeSpecificAvailable
            };
        }
    }
}

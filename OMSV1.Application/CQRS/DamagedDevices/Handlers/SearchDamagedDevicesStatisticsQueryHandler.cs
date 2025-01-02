using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.DamagedDevices;
using System;
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
            try
            {
                // Use the specification to filter damaged devices
                var damagedDeviceQuery = _damagedDeviceRepo.ListAsQueryable(new FilterDamagedDevicesStatisticsSpecification(
                    officeId: request.OfficeId,
                    governorateId: request.GovernorateId,
                    damagedDeviceTypeId: request.DamagedDeviceTypeId,
                    startDate: request.StartDate,
                    endDate: request.EndDate
                ));

                // Aggregate the data by office
                var totalDamagedDevices = await damagedDeviceQuery
                    .GroupBy(d => d.OfficeId)
                    .Select(group => new
                    {
                        OfficeId = group.Key,
                        AvailableDamagedDevices = group.Count(d =>
                            (!request.StartDate.HasValue || d.Date >= request.StartDate.Value) &&
                            (!request.EndDate.HasValue || d.Date <= request.EndDate.Value)),
                        AvailableSpecificDamagedDevices = group.Count(d =>
                            d.DamagedDeviceTypeId == request.DamagedDeviceTypeId &&
                            (!request.StartDate.HasValue || d.Date >= request.StartDate.Value) &&
                            (!request.EndDate.HasValue || d.Date <= request.EndDate.Value))
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

                // Calculate totals
                var availableDamagedDevicesCount = totalDamagedDevices.Sum(d => d.AvailableDamagedDevices);
                var availableSpecificDamagedDevicesCount = totalDamagedDevices.Sum(d => d.AvailableSpecificDamagedDevices);

                var officeSpecificTotal = totalDamagedDevices
                    .Where(d => d.OfficeId == request.OfficeId)
                    .Sum(d => d.AvailableDamagedDevices);

                var officeSpecificAvailable = totalDamagedDevices
                    .Where(d => d.OfficeId == request.OfficeId)
                    .Sum(d => d.AvailableSpecificDamagedDevices);

                return new DamagedDevicesStatisticsDto
                {
                    AvailableDamagedDevices = availableDamagedDevicesCount,
                    AvailableDamagedDevicesInOffice = officeSpecificTotal,
                    AvailableSpecificDamagedDevices = availableSpecificDamagedDevicesCount,
                    AvailableSpecificDamagedDevicesInOffice = officeSpecificAvailable
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new HandlerException("An error occurred while processing the request for damaged devices statistics.", ex);
            }
        }
    }
}

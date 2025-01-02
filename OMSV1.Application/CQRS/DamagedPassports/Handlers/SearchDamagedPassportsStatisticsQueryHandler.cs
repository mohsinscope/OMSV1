using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.DamagedPassports;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.DamagedPassports
{
    public class SearchDamagedPassportsStatisticsHandler : IRequestHandler<SearchDamagedPassportsStatisticsQuery, DamagedPassportsStatisticsDto>
    {
        private readonly IGenericRepository<DamagedPassport> _damagedPassportRepo;
        private readonly IGenericRepository<Office> _officeRepo;

        public SearchDamagedPassportsStatisticsHandler(
            IGenericRepository<DamagedPassport> damagedPassportRepo,
            IGenericRepository<Office> officeRepo)
        {
            _damagedPassportRepo = damagedPassportRepo;
            _officeRepo = officeRepo;
        }

        public async Task<DamagedPassportsStatisticsDto> Handle(SearchDamagedPassportsStatisticsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Use the specification to filter damaged passports
                var damagedPassportQuery = _damagedPassportRepo.ListAsQueryable(new FilterDamagedPassportsStatisticsSpecification(
                    officeId: request.OfficeId,
                    governorateId: request.GovernorateId,
                    damagedTypeId: request.DamagedTypeId,
                    startDate: request.StartDate,
                    endDate: request.EndDate
                ));

                // Aggregate the data by office
                var totalDamagedPassports = await damagedPassportQuery
                    .GroupBy(d => d.OfficeId)
                    .Select(group => new
                    {
                        OfficeId = group.Key,
                        AvailableDamagedPassports = group.Count(d => 
                            (!request.StartDate.HasValue || d.Date >= request.StartDate.Value) &&
                            (!request.EndDate.HasValue || d.Date <= request.EndDate.Value)),
                        AvailableSpecificDamagedPassports = group.Count(d =>
                            d.DamagedTypeId == request.DamagedTypeId &&
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
                var availableDamagedPassportsCount = totalDamagedPassports.Sum(d => d.AvailableDamagedPassports);
                var availableSpecificDamagedPassportsCount = totalDamagedPassports.Sum(d => d.AvailableSpecificDamagedPassports);

                var officeSpecificTotal = totalDamagedPassports
                    .Where(d => d.OfficeId == request.OfficeId)
                    .Sum(d => d.AvailableDamagedPassports);

                var officeSpecificAvailable = totalDamagedPassports
                    .Where(d => d.OfficeId == request.OfficeId)
                    .Sum(d => d.AvailableSpecificDamagedPassports);

                return new DamagedPassportsStatisticsDto
                {
                    AvailableDamagedPassports = availableDamagedPassportsCount,
                    AvailableDamagedPassportsInOffice = officeSpecificTotal,
                    AvailableSpecificDamagedPassports = availableSpecificDamagedPassportsCount,
                    AvailableSpecificDamagedPassportsInOffice = officeSpecificAvailable
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new HandlerException("An error occurred while retrieving damaged passports statistics.", ex);
            }
        }
    }
}

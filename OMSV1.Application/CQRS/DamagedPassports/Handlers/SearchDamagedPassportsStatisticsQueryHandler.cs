using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Entities;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.DamagedPassports;
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
            IQueryable<DamagedPassport> damagedPassportQuery;

            // Determine which query to execute based on the filters provided
            if (!request.GovernorateId.HasValue)
            {
                // Fetch all damaged passports in all offices and governorates
                damagedPassportQuery = _damagedPassportRepo.ListAsQueryable(new FilterDamagedPassportsStatisticsSpecification(
                    officeId: request.OfficeId,
                    damagedTypeId: request.DamagedTypeId,
                    date: request.Date
                ));
            }
            else if (request.OfficeId.HasValue)
            {
                // Fetch damaged passports filtered by both governorate and office
                damagedPassportQuery = _damagedPassportRepo.ListAsQueryable(new FilterDamagedPassportsStatisticsSpecification(
                    officeId: request.OfficeId,
                    governorateId: request.GovernorateId,
                    damagedTypeId: request.DamagedTypeId,
                    date: request.Date
                ));
            }
            else
            {
                // Fetch damaged passports filtered by governorate (for all offices in that governorate)
                damagedPassportQuery = _damagedPassportRepo.ListAsQueryable(new FilterDamagedPassportsStatisticsSpecification(
                    governorateId: request.GovernorateId,
                    damagedTypeId: request.DamagedTypeId,
                    date: request.Date
                ));
            }

            // Aggregate the data by office
            var totalDamagedPassports = await damagedPassportQuery
                .GroupBy(d => d.OfficeId)
                .Select(group => new
                {
                    OfficeId = group.Key,
                    AvailableDamagedPassports = group.Count(d => d.Date == request.Date),  // Direct comparison of dates
                    AvailableSpecificDamagedPassports = group.Count(d => d.DamagedTypeId == request.DamagedTypeId && d.Date == request.Date)
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

            // Calculate total and available damaged passports across all offices
            var availableDamagedPassportsCount = totalDamagedPassports.Sum(d => d.AvailableDamagedPassports);
            var availableSpecificDamagedPassportsCount = totalDamagedPassports.Sum(d => d.AvailableSpecificDamagedPassports);

            // If a specific office is provided, calculate the totals for that office
            var officeSpecificTotal = totalDamagedPassports.Where(d => d.OfficeId == request.OfficeId).Sum(d => d.AvailableDamagedPassports);
            var officeSpecificAvailable = totalDamagedPassports.Where(d => d.OfficeId == request.OfficeId).Sum(d => d.AvailableSpecificDamagedPassports);

            return new DamagedPassportsStatisticsDto
            {
                AvailableDamagedPassports = availableDamagedPassportsCount,
                AvailableDamagedPassportsInOffice = officeSpecificTotal,
                AvailableSpecificDamagedPassports = availableSpecificDamagedPassportsCount,
                AvailableSpecificDamagedPassportsInOffice = officeSpecificAvailable
            };
        }
    }
}

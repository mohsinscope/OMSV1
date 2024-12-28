using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Offices
{
    public class GetGovernoratesWithOfficesForDropdownQueryHandler : IRequestHandler<GetGovernoratesWithOfficesForDropdownQuery, List<GovernorateWithOfficesDropdownDto>>
    {
        private readonly IGenericRepository<Governorate> _governorateRepository;

        public GetGovernoratesWithOfficesForDropdownQueryHandler(
            IGenericRepository<Governorate> governorateRepository
            )
        {
            _governorateRepository = governorateRepository;
        }

        public async Task<List<GovernorateWithOfficesDropdownDto>> Handle(GetGovernoratesWithOfficesForDropdownQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve governorates with offices using IUnitOfWork
                var governoratesQuery = _governorateRepository.GetAllAsQueryable();

                if (request.GovernorateId.HasValue)
                {
                    governoratesQuery = governoratesQuery.Where(g => g.Id == request.GovernorateId.Value);
                }

                // Project to the appropriate DTO structure
                var governorates = await governoratesQuery
                    .Select(g => new GovernorateWithOfficesDropdownDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Offices = g.Offices
                            .Select(o => new OfficeDropdownDto
                            {
                                Id = o.Id,
                                Name = o.Name
                            })
                            .ToList()
                    })
                    .ToListAsync(cancellationToken);

                return governorates;
            }
            catch (Exception ex)
            {
                // Handle and throw a custom HandlerException
                throw new HandlerException("An error occurred while fetching governorates and their offices for dropdown.", ex);
            }
        }
    }
}

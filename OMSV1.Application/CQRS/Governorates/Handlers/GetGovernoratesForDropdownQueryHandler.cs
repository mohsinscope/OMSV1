using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Application.Helpers;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernoratesForDropdownQueryHandler : IRequestHandler<GetGovernoratesForDropdownQuery, List<GovernorateDropdownDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGovernoratesForDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GovernorateDropdownDto>> Handle(GetGovernoratesForDropdownQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the governorates using the repository inside unit of work
                var governorates = await _unitOfWork.Repository<Governorate>().GetAllAsync();

                // Map the entities to DTOs
                var governorateDropdownDtos = governorates.Select(g => new GovernorateDropdownDto
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList();

                return governorateDropdownDtos;
            }
            catch (Exception ex)
            {
                // Log and throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while retrieving the governorates for dropdown.", ex);
            }
        }
    }
}

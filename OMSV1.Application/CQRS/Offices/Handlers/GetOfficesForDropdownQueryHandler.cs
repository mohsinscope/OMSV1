using MediatR;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Offices
{
    public class GetOfficesForDropdownQueryHandler : IRequestHandler<GetOfficesForDropdownQuery, List<OfficeDropdownDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOfficesForDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OfficeDropdownDto>> Handle(GetOfficesForDropdownQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the offices using the repository inside unit of work
                var offices = await _unitOfWork.Repository<Office>().GetAllAsync();

                // Order the offices by their Code property
                var orderedOffices = offices.OrderBy(o => o.Code).ToList();

                // Map the ordered entities to DTOs
                var officeDropdownDtos = orderedOffices.Select(o => new OfficeDropdownDto
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

                return officeDropdownDtos;
            }
            catch (Exception ex)
            {
                // Log and throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while retrieving the office data for dropdown.", ex);
            }
        }
    }
}

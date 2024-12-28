using MediatR;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.CQRS.Lov.DamagedPassport
{
    public class GetAllDamagedTypesQueryHandler : IRequestHandler<GetAllDamagedTypesQuery, List<DamagedTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDamagedTypesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DamagedTypeDto>> Handle(GetAllDamagedTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch all damaged types from the repository
                var damagedTypes = await _unitOfWork.Repository<DamagedType>().GetAllAsync();

                // Map the entities to DTOs
                var damagedTypesDto = damagedTypes.Select(dt => new DamagedTypeDto
                {
                    Id = dt.Id,
                    Name = dt.Name,
                    Description = dt.Description
                }).ToList();

                return damagedTypesDto;
            }
            catch (Exception ex)
            {
                // Log and throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while fetching all damaged types.", ex);
            }
        }
    }
}

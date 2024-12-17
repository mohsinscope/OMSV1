using MediatR;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Application.Dtos.LOV;
using OMSV1.Domain.Entities.DamagedPassport; // For DamagedType entity
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.CQRS.Lov.DamagedPassport
{
    public class GetDamagedTypeByIdQueryHandler : IRequestHandler<GetDamagedTypeByIdQuery, DamagedTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDamagedTypeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DamagedTypeDto> Handle(GetDamagedTypeByIdQuery request, CancellationToken cancellationToken)
        {
            // Get the damaged type by id
            var damagedType = await _unitOfWork.Repository<DamagedType>()
                .GetByIdAsync(request.Id);

            if (damagedType == null)
            {
                return null; // Return null if the damaged type does not exist
            }

            // Map to DTO (use AutoMapper if set up)
            var damagedTypeDto = new DamagedTypeDto
            {
                Id = damagedType.Id,
                Name = damagedType.Name,
                Description = damagedType.Description
            };

            return damagedTypeDto;
        }
    }
}

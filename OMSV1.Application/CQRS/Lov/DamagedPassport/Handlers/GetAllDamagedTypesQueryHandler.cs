using MediatR;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.Dtos.LOV;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Entities.DamagedPassport;

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
            var damagedTypes = await _unitOfWork.Repository<DamagedType>().GetAllAsync();
            var damagedTypesDto = damagedTypes.Select(dt => new DamagedTypeDto
            {
                Id = dt.Id,
                Name = dt.Name,
                Description = dt.Description
            }).ToList();

            return damagedTypesDto;
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.GeneralDirectorates;
using OMSV1.Application.Queries.GeneralDirectorates;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.GeneralDirectorates
{
    public class GetGeneralDirectorateByIdQueryHandler : IRequestHandler<GetGeneralDirectorateyByIdQuery, GeneralDirectorateDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGeneralDirectorateByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralDirectorateDto> Handle(GetGeneralDirectorateyByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<GeneralDirectorate>();

            var generalDirectorate = await repo
                .GetAllAsQueryable()                                            // get the IQueryable<GeneralDirectorate>
                .Include(gd => gd.Ministry)                        // include the Ministry nav prop
                .FirstOrDefaultAsync(gd => gd.Id == request.Id, cancellationToken);

            if (generalDirectorate == null)
                throw new KeyNotFoundException($"GeneralDirectorate with ID {request.Id} was not found.");

            return new GeneralDirectorateDto
            {
                Id           = generalDirectorate.Id,
                Name         = generalDirectorate.Name,
                MinistryId   = generalDirectorate.MinistryId,
                MinistryName = generalDirectorate.Ministry.Name,
                DateCreated  = generalDirectorate.DateCreated
            };
        }
    }
}

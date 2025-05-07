using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.GeneralDirectorates;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.GeneralDirectorates
{
    public class GetGeneralDirectoratesByMinistryIdQueryHandler 
        : IRequestHandler<GetGeneralDirectoratesByMinistryIdQuery, IEnumerable<GeneralDirectorateDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGeneralDirectoratesByMinistryIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GeneralDirectorateDto>> Handle(
            GetGeneralDirectoratesByMinistryIdQuery request, 
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<GeneralDirectorate>();

            var list = await repo
                .GetAllAsQueryable()
                .Where(gd => gd.MinistryId == request.MinistryId)
                .Include(gd => gd.Ministry)
                .Select(gd => new GeneralDirectorateDto
                {
                    Id           = gd.Id,
                    Name         = gd.Name,
                    MinistryId   = gd.MinistryId,
                    MinistryName = gd.Ministry.Name,
                    DateCreated  = gd.DateCreated
                })
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}

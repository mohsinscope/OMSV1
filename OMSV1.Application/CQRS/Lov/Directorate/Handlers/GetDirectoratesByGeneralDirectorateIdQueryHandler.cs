// Application/Queries/Directorates/GetDirectoratesByGeneralDirectorateIdQueryHandler.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Directorates;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Directorates
{
    public class GetDirectoratesByGeneralDirectorateIdQueryHandler
        : IRequestHandler<GetDirectoratesByGeneralDirectorateIdQuery, IEnumerable<DirectorateDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDirectoratesByGeneralDirectorateIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DirectorateDto>> Handle(
            GetDirectoratesByGeneralDirectorateIdQuery request,
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Directorate>();

            var list = await repo
                .GetAllAsQueryable()
                .Where(d => d.GeneralDirectorateId == request.GeneralDirectorateId)
                .Include(d => d.GeneralDirectorate)
                .Select(d => new DirectorateDto
                {
                    Id                        = d.Id,
                    Name                      = d.Name,
                    GeneralDirectorateId      = d.GeneralDirectorateId,
                    GeneralDirectorateName    = d.GeneralDirectorate.Name,
                    DateCreated               = d.DateCreated
                })
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}

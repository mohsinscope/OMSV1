// Application/Queries/Sections/GetSectionsByDepartmentIdQueryHandler.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Sections;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Sections
{
    public class GetSectionsByDepartmentIdQueryHandler
        : IRequestHandler<GetSectionsByDepartmentIdQuery, IEnumerable<SectionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSectionsByDepartmentIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SectionDto>> Handle(
            GetSectionsByDepartmentIdQuery request,
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Section>();

            var list = await repo
                .GetAllAsQueryable()
                .Where(s => s.DepartmentId == request.DepartmentId)
                .Include(s => s.Department)
                .Select(s => new SectionDto
                {
                    Id             = s.Id,
                    Name           = s.Name,
                    DepartmentId   = s.DepartmentId,
                    DepartmentName = s.Department.Name,
                    DateCreated    = s.DateCreated
                })
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Departments;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Departments
{
    public class GetDepartmentsByDirectorateIdQueryHandler
        : IRequestHandler<GetDepartmentsByDirectorateIdQuery, IEnumerable<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDepartmentsByDirectorateIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentDto>> Handle(
            GetDepartmentsByDirectorateIdQuery request,
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Department>();

            var list = await repo
                .GetAllAsQueryable()
                .Where(d => d.DirectorateId == request.DirectorateId)
                .Include(d => d.Directorate)
                .Select(d => new DepartmentDto
                {
                    Id              = d.Id,
                    Name            = d.Name,
                    DirectorateId   = d.DirectorateId,
                    DirectorateName = d.Directorate.Name,
                    DateCreated     = d.DateCreated
                })
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}

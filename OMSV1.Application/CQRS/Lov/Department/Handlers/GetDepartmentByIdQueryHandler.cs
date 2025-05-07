using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Departments;
using OMSV1.Application.Dtos.Directorates;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Departments
{
    public class GetDepartmentsByIdQueryHandler : IRequestHandler<GetDepartmentsByIdQuery, DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDepartmentsByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DepartmentDto> Handle(GetDepartmentsByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Department>();

            var Directorate = await repo
                .GetAllAsQueryable()                                            // get the IQueryable<Directorate>
                .Include(gd => gd.Directorate)                        // include the GeneralDirectorate nav prop
                .FirstOrDefaultAsync(gd => gd.Id == request.Id, cancellationToken);

            if (Directorate == null)
                throw new KeyNotFoundException($"Directorate with ID {request.Id} was not found.");

            return new DepartmentDto
            {
                Id           = Directorate.Id,
                Name         = Directorate.Name,
                DirectorateId  = Directorate.DirectorateId,
                DirectorateName = Directorate.Directorate.Name,
                DateCreated  = Directorate.DateCreated
            };
        }
    }
}

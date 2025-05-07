using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Departments;
using OMSV1.Application.Dtos.Directorates;
using OMSV1.Application.Dtos.Sections;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Sections
{
    public class GetSectionsByIdQueryHandler : IRequestHandler<GetSectionsByIdQuery, SectionDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSectionsByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    public async Task<SectionDto> Handle(GetSectionsByIdQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Section>();

        var section = await repo
            .GetAllAsQueryable()
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (section == null)
            throw new KeyNotFoundException($"Section with ID {request.Id} was not found.");

        return new SectionDto
        {
            Id             = section.Id,
            Name           = section.Name,
            DepartmentId   = section.DepartmentId,
            DepartmentName = section.Department.Name,
            DateCreated    = section.DateCreated
        };
    }
    }
}

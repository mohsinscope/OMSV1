using MediatR;
using OMSV1.Application.Dtos.Departments;

namespace OMSV1.Application.Queries.Departments
{
    public class GetDepartmentsByIdQuery : IRequest<DepartmentDto>
    {
        public Guid Id { get; }

        public GetDepartmentsByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));

            Id = id;
        }
    }
}

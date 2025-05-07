using MediatR;
using OMSV1.Application.Dtos.Departments;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Departments
{
    public class GetAllDepartmentsQuery : IRequest<PagedList<DepartmentDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDepartmentsQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}

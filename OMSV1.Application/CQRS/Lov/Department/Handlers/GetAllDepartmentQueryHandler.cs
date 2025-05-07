using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Departments;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Departments;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Departments
{
    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, PagedList<DepartmentDto>>
    {
        private readonly IGenericRepository<Department> _repository;
        private readonly IMapper _mapper;

        public GetAllDepartmentsQueryHandler(IGenericRepository<Department> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all DepartmentDto records as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project the queryable to DepartmentDto using AutoMapper
                var mappedQuery = queryable.ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider);

                // Create a paginated list using the provided pagination parameters
                var pagedDepartments = await PagedList<DepartmentDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedDepartments;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all Department.", ex);
            }
        }
    }
}

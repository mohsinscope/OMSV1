using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Expenses;

namespace OMSV1.Application.Handlers.Expenses
{
    public class GetFilteredMonthlyExpensesQueryHandler : IRequestHandler<GetFilteredMonthlyExpensesQuery, PagedList<MonthlyExpensesDto>>
    {
        private readonly IGenericRepository<MonthlyExpenses> _repository;
        private readonly IMapper _mapper;

        public GetFilteredMonthlyExpensesQueryHandler(IGenericRepository<MonthlyExpenses> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<MonthlyExpensesDto>> Handle(GetFilteredMonthlyExpensesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Create the specification based on the query parameters
                var spec = new FilterExpensesSpecification(
                    officeId: request.OfficeId,
                    governorateId: request.GovernorateId,
                    profileId: request.ProfileId,
                    status: request.Status,
                    startDate: request.StartDate,
                    endDate: request.EndDate,
                    pageNumber: request.PaginationParams.PageNumber,
                    pageSize: request.PaginationParams.PageSize
                );

                // Get the queryable list of MonthlyExpenses entities
                var queryableResult = _repository.ListAsQueryable(spec);

                // Map to MonthlyExpensesDto
                var mappedQuery = queryableResult.ProjectTo<MonthlyExpensesDto>(_mapper.ConfigurationProvider);

                // Create a paginated list of MonthlyExpensesDto
                return await PagedList<MonthlyExpensesDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors and throw a custom exception
                throw new HandlerException("An error occurred while retrieving monthly expenses.", ex);
            }
        }
    }
}

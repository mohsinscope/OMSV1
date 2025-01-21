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
    public class GetFilteredMonthlyExpensesQueryHandler : 
        IRequestHandler<GetFilteredMonthlyExpensesQuery, PagedList<MonthlyExpensesDto>>
    {
        private readonly IGenericRepository<MonthlyExpenses> _repository;
        private readonly IMapper _mapper;

        public GetFilteredMonthlyExpensesQueryHandler(
            IGenericRepository<MonthlyExpenses> repository, 
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedList<MonthlyExpensesDto>> Handle(
            GetFilteredMonthlyExpensesQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                // Validate request
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                if (request.PaginationParams == null)
                    throw new ArgumentNullException(nameof(request.PaginationParams));

                // Validate date range if both dates are provided
                if (request.StartDate.HasValue && request.EndDate.HasValue)
                {
                    if (request.StartDate > request.EndDate)
                        throw new ArgumentException("Start date cannot be later than end date");
                }

                // Create specification with filters
                var spec = new FilterExpensesSpecification(
                    officeId: request.OfficeId,
                    governorateId: request.GovernorateId,
                    profileId: request.ProfileId,
                    statuses: request.Statuses,
                    startDate: request.StartDate,
                    endDate: request.EndDate
                );

                // Get queryable result
                var queryableResult = _repository.ListAsQueryable(spec);
                if (queryableResult == null)
                    throw new InvalidOperationException("Failed to retrieve expenses from repository");

                // Project to DTO
                var mappedQuery = queryableResult.ProjectTo<MonthlyExpensesDto>(
                    _mapper.ConfigurationProvider
                );

                // Create paginated result
                var pagedResult = await PagedList<MonthlyExpensesDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                // Validate the created paged list
                if (pagedResult == null)
                    throw new InvalidOperationException("Failed to create paged list of expenses");

                return pagedResult;
            }
            catch (ArgumentNullException ex)
            {
                throw new HandlerException($"Invalid argument provided: {ex.ParamName}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new HandlerException("Invalid argument values provided", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new HandlerException("Operation failed while processing expenses", ex);
            }
            catch (Exception ex)
            {
                throw new HandlerException(
                    "An unexpected error occurred while retrieving monthly expenses", 
                    ex
                );
            }
        }
    }

}
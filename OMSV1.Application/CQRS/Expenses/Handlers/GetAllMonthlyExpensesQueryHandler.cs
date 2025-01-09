using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Expenses;

public class GetAllMonthlyExpensesQueryHandler : IRequestHandler<GetAllMonthlyExpensesQuery, PagedList<MonthlyExpensesDto>>
{
    private readonly IGenericRepository<MonthlyExpenses> _repository;
    private readonly IMapper _mapper;

    public GetAllMonthlyExpensesQueryHandler(IGenericRepository<MonthlyExpenses> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedList<MonthlyExpensesDto>> Handle(GetAllMonthlyExpensesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the queryable for MonthlyExpenses
            var monthlyExpensesQuery = _repository.GetAllAsQueryable();

            // Use AutoMapper to project to DTOs
            var mappedQuery = monthlyExpensesQuery.ProjectTo<MonthlyExpensesDto>(_mapper.ConfigurationProvider);

            // Apply pagination
            var pagedMonthlyExpenses = await PagedList<MonthlyExpensesDto>.CreateAsync(
                mappedQuery,
                request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize
            );

            return pagedMonthlyExpenses;
        }
        catch (Exception ex)
        {
            // Handle and throw a custom exception
            throw new HandlerException("An error occurred while fetching monthly expenses.", ex);
        }
    }
}

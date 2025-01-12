using AutoMapper;
using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Expenses;

namespace OMSV1.Application.Handlers.Expenses;

public class SearchExpensesStatisticsHandler : IRequestHandler<SearchExpensesStatisticsQuery, ExpensesStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const decimal Budget = 500000m; // Static budget value

    public SearchExpensesStatisticsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ExpensesStatisticsDto> Handle(SearchExpensesStatisticsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Create specification for filtering
            var specification = new FilterExpensesByThresholdSpecification(
                request.OfficeId,
                request.GovernorateId,
                request.ThresholdId,
                request.StartDate,
                request.EndDate
            );

            // Create a copy of the specification for total count (without pagination)
            var countSpecification = new FilterExpensesByThresholdSpecification(
                request.OfficeId,
                request.GovernorateId,
                request.ThresholdId,
                request.StartDate,
                request.EndDate
            );

            // Fetch filtered data
            var expenses = await _unitOfWork.Repository<MonthlyExpenses>()
                .ListAsync(specification);

            // Total count (ignoring pagination)
            var totalCount = await _unitOfWork.Repository<MonthlyExpenses>()
                .CountAsync(countSpecification);

            // Total amount calculation
            var totalAmount = expenses.Sum(x => x.TotalAmount);

            // Map to DTO with percentage calculation
            var expensesDto = expenses.Select(e => new MonthlyCleanDto
            {
                TotalAmount = e.TotalAmount,
                OfficeName = e.Office?.Name ?? string.Empty,
                GovernorateName = e.Governorate?.Name ?? string.Empty,
                ThresholdName = e.Threshold?.Name ?? string.Empty,
                PercentageOfBudget = Budget > 0 ? Math.Round((e.TotalAmount / Budget) * 100, 2) : 0
            }).ToList();

            // Return the results
            return new ExpensesStatisticsDto
            {
                TotalCount = totalCount,
                TotalAmount = totalAmount,
                Expenses = expensesDto
            };
        }
        catch (Exception ex)
        {
            // Handle unexpected errors and throw a custom exception
            throw new HandlerException("An error occurred while retrieving expenses statistics.", ex);
        }
    }
}

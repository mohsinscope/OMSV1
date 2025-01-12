using AutoMapper;
using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Expenses;

namespace OMSV1.Application.Handlers.Expenses;

public class CompareMonthlyExpensesHandler : IRequestHandler<CompareMonthlyExpensesQuery, ComparisonExpensesStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const decimal Budget = 500000m; // Static budget value

    public CompareMonthlyExpensesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ComparisonExpensesStatisticsDto> Handle(CompareMonthlyExpensesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Define specifications for this month and last month
            var thisMonthSpec = new FilterExpensesByThresholdSpecification(
                request.OfficeId,
                request.GovernorateId,
                request.ThresholdId,
                request.StartDate,
                request.EndDate
            );

            var lastMonthSpec = new FilterExpensesByThresholdSpecification(
                request.OfficeId,
                request.GovernorateId,
                request.ThresholdId,
                request.StartDate.AddMonths(-1).Date,
                request.EndDate.AddMonths(-1).Date.AddDays(1)
            );

            // Fetch data for this month and last month
            var thisMonthExpenses = await _unitOfWork.Repository<MonthlyExpenses>().ListAsync(thisMonthSpec);
            var lastMonthExpenses = await _unitOfWork.Repository<MonthlyExpenses>().ListAsync(lastMonthSpec);

            // Calculate total amounts
            var thisMonthTotal = thisMonthExpenses.Sum(x => x.TotalAmount);
            var lastMonthTotal = lastMonthExpenses.Sum(x => x.TotalAmount);

            // Calculate percentages
            var thisMonthPercentage = Budget > 0 
                ? Math.Round((thisMonthTotal / Budget) * 100, 2) 
                : 0;

            var lastMonthPercentage = Budget > 0 
                ? Math.Round((lastMonthTotal / Budget) * 100, 2) 
                : 0;

            // Map to DTOs
            var thisMonthDto = thisMonthExpenses.Select(e => new MonthlyCleanDto
            {
                TotalAmount = e.TotalAmount,
                OfficeName = e.Office?.Name ?? string.Empty,
                GovernorateName = e.Governorate?.Name ?? string.Empty,
                ThresholdName = e.Threshold?.Name ?? string.Empty,
                PercentageOfBudget = Budget > 0 ? Math.Round((e.TotalAmount / Budget) * 100, 2) : 0
            }).ToList();

            var lastMonthDto = lastMonthExpenses.Select(e => new MonthlyCleanDto
            {
                TotalAmount = e.TotalAmount,
                OfficeName = e.Office?.Name ?? string.Empty,
                GovernorateName = e.Governorate?.Name ?? string.Empty,
                ThresholdName = e.Threshold?.Name ?? string.Empty,
                PercentageOfBudget = Budget > 0 ? Math.Round((e.TotalAmount / Budget) * 100, 2) : 0
            }).ToList();

            // Return comparison statistics
            return new ComparisonExpensesStatisticsDto
            {
                ThisMonth = new ExpensesStatisticsDto
                {
                    TotalCount = thisMonthExpenses.Count,
                    TotalAmount = thisMonthTotal,
                    TotalPercentage = thisMonthPercentage,
                    Expenses = thisMonthDto
                },
                LastMonth = new ExpensesStatisticsDto
                {
                    TotalCount = lastMonthExpenses.Count,
                    TotalAmount = lastMonthTotal,
                    TotalPercentage = lastMonthPercentage,
                    Expenses = lastMonthDto
                }
            };
        }
        catch (Exception ex)
        {
            // Handle unexpected errors and throw a custom exception
            throw new HandlerException("An error occurred while comparing monthly expenses.", ex);
        }
    }
}
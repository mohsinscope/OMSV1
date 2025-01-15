using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Expenses;

namespace OMSV1.Application.Handlers.Expenses;

public class GetStatisticsForLastTwoMonthsHandler : IRequestHandler<GetStatisticsForLastTwoMonthsQuery, ExpensesStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetStatisticsForLastTwoMonthsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ExpensesStatisticsDto> Handle(GetStatisticsForLastTwoMonthsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.Repository<MonthlyExpenses>();

            // Fetch the last two completed expenses, ordered by DateCreated
            var expenses = await repository.GetAllAsQueryable()
                .Where(x => x.Status == Status.Completed)
                .OrderByDescending(x => x.DateCreated)
                .Take(2)
                .Include(x => x.Office)
                .Include(x => x.Governorate)
                .Include(x => x.Threshold)
                .ToListAsync(cancellationToken);

            if (expenses.Count < 2)
            {
                throw new Exception("Not enough data to compare the last two months of expenses.");
            }

            // Prepare the list of expenses, calculating the percentage based on the office budget
            var mappedExpenses = expenses.Select(e =>
            {
                var officeBudget = e.Office?.Budget ?? 0; // Default to 0 if no budget is set
                return new MonthlyCleanDto
                {
                    DateCreated = e.DateCreated,
                    TotalAmount = e.TotalAmount,
                    OfficeName = e.Office?.Name ?? "Unknown Office",
                    GovernorateName = e.Governorate?.Name ?? "Unknown Governorate",
                    ThresholdName = e.Threshold?.Name ?? "No Threshold",
                    PercentageOfBudget = officeBudget > 0 ? Math.Round((e.TotalAmount / officeBudget) * 100, 2) : 0,
                };
            }).ToList();

            // Calculate aggregated statistics
            var totalAmount = expenses.Sum(x => x.TotalAmount);
            var totalBudget = expenses.Sum(x => x.Office?.Budget ?? 0); // Aggregate budget from all offices
            var totalPercentage = totalBudget > 0 ? Math.Round((totalAmount / totalBudget) * 100, 2) : 0;

            // Prepare response
            return new ExpensesStatisticsDto
            {
                TotalCount = expenses.Count,
                TotalAmount = totalAmount,
                TotalPercentage = totalPercentage,
                Expenses = mappedExpenses
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw new HandlerException("An error occurred while calculating statistics for the last two months.", ex);
        }
    }
}

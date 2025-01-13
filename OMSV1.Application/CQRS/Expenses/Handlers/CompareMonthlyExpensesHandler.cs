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
    private const decimal Budget = 500000m; // Static budget value

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
            .OrderByDescending(x => x.DateCreated)  // Ensure we get the most recent ones first
            .Take(2)  // Only take the last two
            .Include(x => x.Office)
            .Include(x => x.Governorate)
            .Include(x => x.Threshold)
            .ToListAsync(cancellationToken);

        // If there are less than 2 expenses, handle accordingly
        if (expenses.Count < 2)
        {
            throw new Exception("Not enough data to compare the last two months of expenses.");
        }

        // Prepare the list of expenses, marking the current and last month
        var mappedExpenses = expenses.Select(e => new MonthlyCleanDto
        {
            DateCreated = e.DateCreated,  // Include DateCreated in the response
            TotalAmount = e.TotalAmount,
            OfficeName = e.Office?.Name ?? "Unknown Office",
            GovernorateName = e.Governorate?.Name ?? "Unknown Governorate",
            ThresholdName = e.Threshold?.Name ?? "No Threshold",
            PercentageOfBudget = Budget > 0 ? Math.Round((e.TotalAmount / Budget) * 100, 2) : 0,
        }).ToList();

        // Prepare response
        return new ExpensesStatisticsDto
        {
            TotalCount = expenses.Count,
            TotalAmount = expenses.Sum(x => x.TotalAmount),
            TotalPercentage = Budget > 0 ? Math.Round((expenses.Sum(x => x.TotalAmount) / Budget) * 100, 2) : 0,
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

using AutoMapper;
using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Expenses;

namespace OMSV1.Application.Handlers.Expenses
{
    public class GetExpenseReportQueryHandler : IRequestHandler<GetExpenseReportQuery, ExpenseReportDto>
    {
        private readonly IGenericRepository<MonthlyExpenses> _repository;
        private readonly IMapper _mapper;

        public GetExpenseReportQueryHandler(IGenericRepository<MonthlyExpenses> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ExpenseReportDto> Handle(GetExpenseReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Create the specification
                var spec = new FilterExpensesSpecification(
                    governorateId: request.GovernorateId,
                    officeId: request.OfficeId,
                    startDate: request.StartDate,
                    endDate: request.EndDate
                );

                // Get the filtered list of expenses
                var expenses = await _repository.ListAsync(spec);

                // Filter only completed expenses
                var completedExpenses = expenses.Where(e => e.Status == Domain.Enums.Status.Completed).ToList();

                // Total count
                var totalCount = completedExpenses.Count;

                // Total amount
                var totalAmount = completedExpenses.Sum(x => x.TotalAmount);

                // Total percentage of budget (sum all office budgets involved)
                var totalBudget = completedExpenses.Sum(x => x.Office?.Budget ?? 0);
                var totalPercentage = totalBudget > 0 ? Math.Round((double)(totalAmount / totalBudget) * 100, 2) : 0;

                // Map to detailed expense DTOs
                var expenseDetails = completedExpenses.Select(e =>
                {
                    var officeBudget = e.Office?.Budget ?? 0; // Office-specific budget
                    return new MonthlyExpenseDetailDto
                    {
                        TotalAmount = e.TotalAmount,
                        OfficeName = e.Office?.Name ?? string.Empty,
                        GovernorateName = e.Governorate?.Name ?? string.Empty,
                        PercentageOfBudget = officeBudget > 0 ? Math.Round((double)(e.TotalAmount / officeBudget) * 100, 2) : 0,
                        DateCreated = e.DateCreated
                    };
                }).ToList();

                return new ExpenseReportDto
                {
                    TotalCount = totalCount,
                    TotalAmount = totalAmount,
                    TotalPercentage = totalPercentage,
                    Expenses = expenseDetails
                };
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while retrieving the expense report.", ex);
            }
        }
    }
}

namespace OMSV1.Application.Handlers.Expenses;

using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

public class AddDailyExpensesCommandHandler : IRequestHandler<AddDailyExpensesCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddDailyExpensesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddDailyExpensesCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the associated MonthlyExpenses entity
        var monthlyExpenses = await _unitOfWork.Repository<MonthlyExpenses>().GetByIdAsync(request.MonthlyExpensesId);

        if (monthlyExpenses == null)
        {
            throw new KeyNotFoundException($"MonthlyExpenses with ID {request.MonthlyExpensesId} not found.");
        }

        if (monthlyExpenses.Status != Status.New && monthlyExpenses.Status != Status.ReturnedToSupervisor)
        {
            throw new InvalidOperationException("Cannot add daily expenses to a MonthlyExpenses that is not in Pending status.");
        }

        // Create a new DailyExpenses object
            var dailyExpense = new DailyExpenses(
            request.Price,
            request.Quantity,
            request.Price * request.Quantity, // Calculate Amount
            request.Notes,
            DateTime.SpecifyKind(request.ExpenseDate, DateTimeKind.Utc), // Ensure UTC
            request.ExpenseTypeId,
            request.MonthlyExpensesId
        );


        // Add the DailyExpenses entity to the repository
        await _unitOfWork.Repository<DailyExpenses>().AddAsync(dailyExpense);

        // Add the DailyExpense to the MonthlyExpenses
        monthlyExpenses.AddDailyExpense(dailyExpense);

        // Save changes to the database
        if (!await _unitOfWork.SaveAsync(cancellationToken))
        {
            throw new Exception("Failed to save DailyExpenses to the database.");
        }

        return dailyExpense.Id;
    }
}

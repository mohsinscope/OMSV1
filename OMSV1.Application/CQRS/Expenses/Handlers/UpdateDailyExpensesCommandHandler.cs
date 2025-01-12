using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Expenses;

public class UpdateDailyExpensesCommandHandler : IRequestHandler<UpdateDailyExpensesCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDailyExpensesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateDailyExpensesCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the existing DailyExpenses entity
        var dailyExpense = await _unitOfWork.Repository<DailyExpenses>().GetByIdAsync(request.Id);

        if (dailyExpense == null)
        {
            throw new KeyNotFoundException($"DailyExpenses with ID {request.Id} not found.");
        }

        // Retrieve the associated MonthlyExpenses entity
        var monthlyExpenses = await _unitOfWork.Repository<MonthlyExpenses>().GetByIdAsync(dailyExpense.MonthlyExpensesId);

        if (monthlyExpenses == null)
        {
            throw new KeyNotFoundException($"MonthlyExpenses with ID {dailyExpense.MonthlyExpensesId} not found.");
        }
            // Check if the MonthlyExpenses is in allowed statuses (New or ReturnedToSupervisor)
        if (monthlyExpenses.Status != Status.New && monthlyExpenses.Status != Status.ReturnedToSupervisor)
        {
            throw new InvalidOperationException("Cannot update daily expenses for a MonthlyExpenses that is not in an allowed status.");
        }


        // Adjust MonthlyExpenses.TotalAmount by removing the old amount first
        monthlyExpenses.AdjustTotalAmount(-dailyExpense.Amount);

        // Update the properties of the existing DailyExpenses entity
        dailyExpense.Update(
            request.Price,
            request.Quantity,
            request.Price * request.Quantity, // Calculate the new amount
            request.Notes,
            request.ExpenseDate,
            request.ExpenseTypeId
        );

        // Recalculate MonthlyExpenses.TotalAmount with the updated amount
        monthlyExpenses.AdjustTotalAmount(dailyExpense.Amount);

        // Save changes
        if (!await _unitOfWork.SaveAsync(cancellationToken))
        {
            throw new Exception("Failed to update the DailyExpenses.");
        }

        return true;
    }
}

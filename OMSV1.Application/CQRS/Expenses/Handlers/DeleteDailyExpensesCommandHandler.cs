namespace OMSV1.Application.Handlers.Expenses;

using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

public class DeleteDailyExpensesCommandHandler : IRequestHandler<DeleteDailyExpensesCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDailyExpensesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteDailyExpensesCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the DailyExpenses entity
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

        // Check if the MonthlyExpenses is in Pending status
        if (monthlyExpenses.Status != Status.New && monthlyExpenses.Status != Status.ReturnedToSupervisor)
        {
            throw new InvalidOperationException("Cannot delete daily expenses from a MonthlyExpenses that is not in Pending status.");
        }

        // Adjust the TotalAmount
        monthlyExpenses.AdjustTotalAmount(-dailyExpense.Amount);

        // Remove the DailyExpenses
        await _unitOfWork.Repository<DailyExpenses>().DeleteAsync(dailyExpense);

        // Save changes to the database
        if (!await _unitOfWork.SaveAsync(cancellationToken))
        {
            throw new Exception("Failed to delete the DailyExpenses.");
        }

        return true;
    }
}

using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Expenses
{
    public class UpdateDailyExpensesCommandHandler : IRequestHandler<UpdateDailyExpensesCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDailyExpensesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDailyExpensesCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the existing DailyExpenses entity.
            var dailyExpense = await _unitOfWork.Repository<DailyExpenses>().GetByIdAsync(request.Id);
            if (dailyExpense == null)
            {
                throw new KeyNotFoundException($"DailyExpenses with ID {request.Id} not found.");
            }

            // Retrieve the associated MonthlyExpenses entity.
            var monthlyExpenses = await _unitOfWork.Repository<MonthlyExpenses>().GetByIdAsync(dailyExpense.MonthlyExpensesId);
            if (monthlyExpenses == null)
            {
                throw new KeyNotFoundException($"MonthlyExpenses with ID {dailyExpense.MonthlyExpensesId} not found.");
            }

            // Ensure the MonthlyExpenses is in an allowed status.
            if (monthlyExpenses.Status != Status.New && monthlyExpenses.Status != Status.ReturnedToSupervisor)
            {
                throw new InvalidOperationException("Cannot update daily expenses for a MonthlyExpenses that is not in an allowed status.");
            }

            // Record the parent's current total.
            var oldTotal = dailyExpense.GetTotalAmount();

            // Update the DailyExpenses entity (the parent's own properties).
            dailyExpense.Update(
                request.Price,
                request.Quantity,
                request.Notes,
                DateTime.SpecifyKind(request.ExpenseDate, DateTimeKind.Utc),
                request.ExpenseTypeId
            );

            // Process subexpense updates, if any.
            if (request.SubExpenseUpdates != null && request.SubExpenseUpdates.Any())
            {
                foreach (var subUpdate in request.SubExpenseUpdates)
                {
                    if (subUpdate.Id.HasValue && subUpdate.Id.Value != Guid.Empty)
                    {
                        // Find the existing subexpense.
                        var existingSubExpense = dailyExpense.SubExpenses.FirstOrDefault(s => s.Id == subUpdate.Id.Value);
                        if (existingSubExpense != null)
                        {
                            // Use the provided ExpenseDate if given; otherwise, keep the existing one.
                            var newExpenseDate = subUpdate.ExpenseDate.HasValue 
                                ? DateTime.SpecifyKind(subUpdate.ExpenseDate.Value, DateTimeKind.Utc)
                                : existingSubExpense.ExpenseDate;

                            // Update the subexpense.
                            existingSubExpense.Update(
                                subUpdate.Price,
                                subUpdate.Quantity,
                                subUpdate.Notes,
                                newExpenseDate,
                                subUpdate.ExpenseTypeId
                            );
                        }
                        else
                        {
                            throw new KeyNotFoundException($"Subexpense with ID {subUpdate.Id} not found.");
                        }
                    }
                    else
                    {
                        // No valid Id provided, so treat this as a new subexpense.
                        dailyExpense.AddSubExpense(
                            subUpdate.Price,
                            subUpdate.Quantity,
                            subUpdate.Notes,
                            subUpdate.ExpenseTypeId
                        );
                    }
                }
            }

            // Recalculate the new total for the parent.
            var newTotal = dailyExpense.GetTotalAmount();

            // Adjust MonthlyExpenses.TotalAmount by the difference.
            monthlyExpenses.AdjustTotalAmount(newTotal - oldTotal);

            // Save changes to the database.
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to update the DailyExpenses.");
            }

            return true;
        }
    }
}

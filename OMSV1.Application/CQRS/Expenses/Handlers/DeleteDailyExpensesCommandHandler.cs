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
    public class DeleteDailyExpensesCommandHandler : IRequestHandler<DeleteDailyExpensesCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDailyExpensesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDailyExpensesCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the DailyExpenses entity (parent).
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

            // Check that MonthlyExpenses is in a Pending status.
            if (monthlyExpenses.Status != Status.New && monthlyExpenses.Status != Status.ReturnedToSupervisor)
            {
                throw new InvalidOperationException("Cannot delete daily expenses from a MonthlyExpenses that is not in Pending status.");
            }

            // Retrieve all subexpenses that have this daily expense as their parent.
            var allDailyExpenses = await _unitOfWork.Repository<DailyExpenses>().GetAllAsync();
            var subExpenses = allDailyExpenses.Where(x => x.ParentExpenseId == dailyExpense.Id).ToList();

            // Calculate the total amount to subtract from the monthly expense.
            // The monthly total was originally increased by:
            //   - The parent's amount (dailyExpense.Amount) via AddDailyExpense
            //   - Plus, if subexpenses existed, an additional amount.
            // Here we subtract the parent's amount plus the sum of all subexpenses.
            decimal totalToSubtract = dailyExpense.Amount;
            if (subExpenses.Any())
            {
                totalToSubtract += subExpenses.Sum(se => se.Amount);
                // Delete all subexpenses.
                foreach (var subExpense in subExpenses)
                {
                    await _unitOfWork.Repository<DailyExpenses>().DeleteAsync(subExpense);
                }
            }

            // Adjust the MonthlyExpenses total.
            monthlyExpenses.AdjustTotalAmount(-totalToSubtract);

            // Delete the parent daily expense.
            await _unitOfWork.Repository<DailyExpenses>().DeleteAsync(dailyExpense);

            // Save changes to the database.
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to delete the DailyExpenses.");
            }

            return true;
        }
    }
}

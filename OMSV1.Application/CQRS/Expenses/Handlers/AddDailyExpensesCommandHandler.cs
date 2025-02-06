using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Expenses
{
    public class AddDailyExpensesCommandHandler : IRequestHandler<AddDailyExpensesCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddDailyExpensesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddDailyExpensesCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the associated MonthlyExpenses entity.
            var monthlyExpenses = await _unitOfWork.Repository<MonthlyExpenses>()
                .GetByIdAsync(request.MonthlyExpensesId);

            if (monthlyExpenses == null)
            {
                throw new KeyNotFoundException($"MonthlyExpenses with ID {request.MonthlyExpensesId} not found.");
            }

            if (monthlyExpenses.Status != Status.New && monthlyExpenses.Status != Status.ReturnedToSupervisor)
            {
                throw new InvalidOperationException("Cannot add daily expenses to a MonthlyExpenses that is not in Pending status.");
            }

            // Create the parent DailyExpenses object.
            var dailyExpense = new DailyExpenses(
                request.Price,
                request.Quantity,
                request.Notes,
                DateTime.SpecifyKind(request.ExpenseDate, DateTimeKind.Utc), // Ensure UTC
                request.ExpenseTypeId,
                request.MonthlyExpensesId
            );

            // If subexpenses are provided, add each with its own ExpenseTypeId.
            if (request.SubExpenses != null && request.SubExpenses.Count > 0)
            {
                foreach (var item in request.SubExpenses)
                {
                    dailyExpense.AddSubExpense(item.Price, item.Quantity, item.Notes, item.ExpenseTypeId);
                }
            }

            // Add the DailyExpenses entity to the repository.
            await _unitOfWork.Repository<DailyExpenses>().AddAsync(dailyExpense);

            // Associate the DailyExpense with the MonthlyExpenses and update threshold, if applicable.
            monthlyExpenses.AddDailyExpense(dailyExpense, await _unitOfWork.Repository<Threshold>().GetAllAsync());

            // Update the MonthlyExpenses in the repository.
            await _unitOfWork.Repository<MonthlyExpenses>().UpdateAsync(monthlyExpenses);

            // Save changes to the database.
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to save DailyExpenses to the database.");
            }

            return dailyExpense.Id;
        }
    }
}

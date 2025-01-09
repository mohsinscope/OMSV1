
using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Expenses;

public class DeleteExpenseTypeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteExpenseTypeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteExpenseTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the ExpenseType entity
            var expenseType = await _unitOfWork.Repository<ExpenseType>().GetByIdAsync(request.Id);

            if (expenseType == null)
            {
                throw new KeyNotFoundException($"ExpenseType with ID {request.Id} not found.");
            }

            // Delete the entity
            await _unitOfWork.Repository<ExpenseType>().DeleteAsync(expenseType);

            // Save changes
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to delete the Expense Type from the database.");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new HandlerException("An error occurred while deleting the Expense Type.", ex);
        }
    }
}

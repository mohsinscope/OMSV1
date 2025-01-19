using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Expenses
{
    public class AddActionCommandHandler : IRequestHandler<AddActionCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddActionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.ActionType))
                    throw new ArgumentException("ActionType cannot be empty.");

                if (string.IsNullOrWhiteSpace(request.Notes))
                    throw new ArgumentException("Notes cannot be empty.");

                // Check if MonthlyExpenses exists
                var monthlyExpensesExists = await _unitOfWork.Repository<MonthlyExpenses>().ExistsAsync(me => me.Id == request.MonthlyExpensesId);
                if (!monthlyExpensesExists)
                    throw new ArgumentException($"MonthlyExpenses with ID {request.MonthlyExpensesId} does not exist.");

                // Create the Action entity
                var action = new OMSV1.Domain.Entities.Expenses.Action(
                    request.ActionType,
                    request.Notes,
                    request.MonthlyExpensesId
                );

                // Add the Action entity to the repository
                await _unitOfWork.Repository<OMSV1.Domain.Entities.Expenses.Action>().AddAsync(action);

                // Save changes to the database
                var saveResult = await _unitOfWork.SaveAsync(cancellationToken);
                if (!saveResult)
                    throw new Exception("Failed to save the Action to the database.");

                return action.Id; // Return the generated Action ID
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and throw a custom exception
                throw new HandlerException("An error occurred while adding the action.", ex);
            }
        }
    }
}

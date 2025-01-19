using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Expenses
{
    public class UpdateMonthlyExpensesStatusCommandHandler : IRequestHandler<UpdateMonthlyExpensesStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMonthlyExpensesStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateMonthlyExpensesStatusCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the associated MonthlyExpenses entity
            var monthlyExpenses = await _unitOfWork.Repository<MonthlyExpenses>().GetByIdAsync(request.MonthlyExpensesId);

            if (monthlyExpenses == null)
            {
                throw new KeyNotFoundException($"MonthlyExpenses with ID {request.MonthlyExpensesId} not found.");
            }

            if (monthlyExpenses.Status == Status.Completed)
            {
                throw new InvalidOperationException("Cannot change the status of a completed MonthlyExpenses.");
            }

            // Update the status
            monthlyExpenses.UpdateStatus((Status)request.NewStatus);

            // Save changes to the database
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to update MonthlyExpenses status.");
            }

            return true;
        }
    }
}

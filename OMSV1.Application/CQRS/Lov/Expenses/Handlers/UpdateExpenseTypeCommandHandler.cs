using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
namespace OMSV1.Application.Handlers.Expenses;

public class UpdateExpenseTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateExpenseTypeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(UpdateExpenseTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the ExpenseType entity
var expenseType = await _unitOfWork.Repository<ExpenseType>().GetByIdAsync(request.Id);

            if (expenseType == null)
            {
                throw new KeyNotFoundException($"ExpenseType with ID {request.Id} not found.");
            }

            // Update the entity's name
            expenseType.UpdateName(request.Name);

            // Save changes
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to update the Expense Type in the database.");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new HandlerException("An error occurred while updating the Expense Type.", ex);
        }
    }
}
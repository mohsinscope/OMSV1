namespace OMSV1.Application.Handlers.Expenses;

using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

public class CreateExpenseTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateExpenseTypeCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(CreateExpenseTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Expense Type Name cannot be empty.");
            }

            // Map the request to the ExpenseType entity
            var expenseType = new ExpenseType(request.Name);

            // Add the ExpenseType to the repository using AddAsync
            await _unitOfWork.Repository<ExpenseType>().AddAsync(expenseType);

            // Save changes to the database
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                // Handle failure to save
                throw new Exception("Failed to save the Expense Type to the database.");
            }

            // Return the ID of the newly created Expense Type
            return expenseType.Id;
        }
        catch (HandlerException ex)
        {
            // Handle specific business exceptions (HandlerException)
            throw new HandlerException("An error occurred while processing the Expense Type creation request.", ex);
        }
        catch (Exception ex)
        {
            // Handle any unexpected exceptions (generic)
            throw new HandlerException("An unexpected error occurred.", ex);
        }
    }
}

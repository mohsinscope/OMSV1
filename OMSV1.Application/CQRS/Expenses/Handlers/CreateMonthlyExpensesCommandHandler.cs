namespace OMSV1.Application.Handlers.Expenses;

using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

public class CreateMonthlyExpensesCommandHandler : IRequestHandler<CreateMonthlyExpensesCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMonthlyExpensesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateMonthlyExpensesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate input
            if (request.TotalAmount <= -1)
            {
                throw new ArgumentException("TotalAmount must be greater than zero.");
            }

            // Map request to entity
            var monthlyExpenses = new MonthlyExpenses(
                (Status)request.Status,
                request.TotalAmount,
                request.Notes,
                request.OfficeId,
                request.GovernorateId,
                request.ProfileId);

            // Add entity to the repository
            await _unitOfWork.Repository<MonthlyExpenses>().AddAsync(monthlyExpenses);

            // Save changes to the database
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to save MonthlyExpenses to the database.");
            }

            return monthlyExpenses.Id;
        }
     catch (Exception ex)
{
    throw new HandlerException($"An error occurred while creating the MonthlyExpenses. Details: {ex.Message}", ex);
}

    }
}

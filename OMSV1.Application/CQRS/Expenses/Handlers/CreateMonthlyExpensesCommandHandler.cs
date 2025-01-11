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
            if (request.TotalAmount < 0)
            {
                throw new ArgumentException("TotalAmount must be greater than or equal to zero.");
            }

            // Fetch all thresholds
            var thresholds = await _unitOfWork.Repository<Threshold>().GetAllAsync();
            if (thresholds == null || !thresholds.Any())
            {
                throw new InvalidOperationException("No thresholds have been defined.");
            }

            // Determine the appropriate threshold
            var threshold = thresholds.FirstOrDefault(t =>
                request.TotalAmount >= t.MinValue && request.TotalAmount <= t.MaxValue);

            if (threshold == null)
            {
                throw new InvalidOperationException("No matching threshold found for the given TotalAmount.");
            }

            // Create MonthlyExpenses with the assigned threshold
            var monthlyExpenses = new MonthlyExpenses(
                (Status)request.Status,
                request.TotalAmount,
                request.Notes,
                request.OfficeId,
                request.GovernorateId,
                request.ProfileId
            );

            // Assign the determined threshold
            monthlyExpenses.AssignThreshold(threshold);

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

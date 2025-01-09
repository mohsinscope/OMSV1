using AutoMapper;
using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Expenses;

public class GetDailyExpensesByMonthlyExpensesIdQueryHandler : IRequestHandler<GetDailyExpensesByMonthlyExpensesIdQuery, DailyExpensesResponseDto>
{
    private readonly IGenericRepository<MonthlyExpenses> _repository;
    private readonly IMapper _mapper;

    public GetDailyExpensesByMonthlyExpensesIdQueryHandler(IGenericRepository<MonthlyExpenses> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DailyExpensesResponseDto> Handle(GetDailyExpensesByMonthlyExpensesIdQuery request, CancellationToken cancellationToken)
    {
        // Fetch MonthlyExpenses with DailyExpenses included
        var monthlyExpenses = await _repository.GetByIdWithIncludesAsync(
            request.MonthlyExpensesId,
            me => me.dailyExpenses
        );

        if (monthlyExpenses == null)
        {
            throw new KeyNotFoundException($"MonthlyExpenses with ID {request.MonthlyExpensesId} not found.");
        }

        // Map DailyExpenses to DTO
        var dailyExpensesDto = _mapper.Map<List<DailyExpensesDto>>(monthlyExpenses.dailyExpenses);

        // Create the response
        var response = new DailyExpensesResponseDto
        {
            MonthlyExpensesId = monthlyExpenses.Id,
            DailyExpenses = dailyExpensesDto
        };

        return response;
    }
}

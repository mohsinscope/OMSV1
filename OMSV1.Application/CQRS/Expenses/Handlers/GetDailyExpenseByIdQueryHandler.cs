using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Expenses
{
    public class GetDailyExpenseByIdQueryHandler : IRequestHandler<GetDailyExpenseByIdQuery, DailyExpensesDto?>
    {
        private readonly IGenericRepository<DailyExpenses> _repository;
        private readonly IMapper _mapper;

        public GetDailyExpenseByIdQueryHandler(IGenericRepository<DailyExpenses> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DailyExpensesDto?> Handle(GetDailyExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the specific DailyExpense as an IQueryable
                var dailyExpenseQuery = _repository.GetAllAsQueryable()
                    .Where(de => de.Id == request.Id); // Filter by the given ID

                // Map to DailyExpensesDto using AutoMapper's ProjectTo
                var dailyExpenseDto = await dailyExpenseQuery
                    .ProjectTo<DailyExpensesDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken); // Fetch the first result or null

                return dailyExpenseDto; // Return the mapped DTO
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors and throw a custom exception
                throw new HandlerException("An error occurred while retrieving the daily expense by ID.", ex);
            }
        }
    }
}

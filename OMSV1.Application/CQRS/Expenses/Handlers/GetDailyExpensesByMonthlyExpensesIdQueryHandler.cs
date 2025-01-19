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
    public class GetDailyExpensesByMonthlyExpensesIdQueryHandler : IRequestHandler<GetDailyExpensesByMonthlyExpensesIdQuery, List<DailyExpensesDto>>
    {
        private readonly IGenericRepository<DailyExpenses> _repository;
        private readonly IMapper _mapper;

        public GetDailyExpensesByMonthlyExpensesIdQueryHandler(IGenericRepository<DailyExpenses> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DailyExpensesDto>> Handle(GetDailyExpensesByMonthlyExpensesIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve DailyExpenses filtered by MonthlyExpensesId
                var dailyExpensesQuery = _repository.GetAllAsQueryable()
                    .Where(de => de.MonthlyExpensesId == request.MonthlyExpensesId) // Filter by MonthlyExpensesId
                    .OrderByDescending(de => de.DateCreated) // Order by DateCreated in descending order
                    .Include(de => de.ExpenseType); // Include ExpenseType for mapping

                // Map to DailyExpensesDto using AutoMapper's ProjectTo
                var dailyExpensesDtoList = await dailyExpensesQuery
                    .ProjectTo<DailyExpensesDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken); // Fetch and project the data

                return dailyExpensesDtoList; // Return the list of mapped DTOs
            }
            catch (HandlerException ex)
            {
                // Log and rethrow the custom exception
                throw new HandlerException("Error occurred while retrieving daily expenses.", ex);
            }
            catch (Exception ex)
            {
                // Catch unexpected errors and rethrow them as HandlerException
                throw new HandlerException("An unexpected error occurred while retrieving daily expenses.", ex);
            }
        }
    }
}

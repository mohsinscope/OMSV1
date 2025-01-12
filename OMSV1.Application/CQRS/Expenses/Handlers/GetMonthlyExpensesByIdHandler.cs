using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Handlers.Expenses
{
    public class GetMonthlyExpensesByIdHandler : IRequestHandler<GetMonthlyExpensesByIdQuery, MonthlyExpensesDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMonthlyExpensesByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MonthlyExpensesDto?> Handle(GetMonthlyExpensesByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Query the database and project to MonthlyExpensesDto
                var query = _unitOfWork.Repository<MonthlyExpenses>().GetAllAsQueryable()
                    .Where(me => me.Id == request.Id)
                    .ProjectTo<MonthlyExpensesDto>(_mapper.ConfigurationProvider);

                // Fetch the monthly expense record
                var monthlyExpense = await query.FirstOrDefaultAsync(cancellationToken);

                if (monthlyExpense == null)
                {
                    throw new HandlerException($"MonthlyExpenses with ID {request.Id} not found.");
                }

                return monthlyExpense;
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and throw a custom exception
                throw new HandlerException("An error occurred while fetching the monthly expenses record.", ex);
            }
        }
    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Queries.Actions;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Actions
{
    public class GetActionsByMonthlyExpensesIdQueryHandler : IRequestHandler<GetActionsByMonthlyExpensesIdQuery, List<ActionDto>>
    {
        private readonly IGenericRepository<OMSV1.Domain.Entities.Expenses.Action> _repository;
        private readonly IMapper _mapper;

        public GetActionsByMonthlyExpensesIdQueryHandler(IGenericRepository<OMSV1.Domain.Entities.Expenses.Action> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ActionDto>> Handle(GetActionsByMonthlyExpensesIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Query to filter actions by MonthlyExpensesId and order by DateCreated descending
                var actionsQuery = _repository.GetAllAsQueryable()
                    .Where(a => a.MonthlyExpensesId == request.MonthlyExpensesId)
                    .OrderByDescending(a => a.DateCreated); // Order by DateCreated descending

                // Project to ActionDto using AutoMapper's ProjectTo
                var actionDtoList = await actionsQuery
                    .ProjectTo<ActionDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return actionDtoList;
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                throw new Exception("Error occurred while retrieving actions.", ex);
            }
        }
    }
}

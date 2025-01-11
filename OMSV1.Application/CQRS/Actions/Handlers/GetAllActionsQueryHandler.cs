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
    public class GetAllActionsQueryHandler : IRequestHandler<GetAllActionsQuery, List<ActionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllActionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ActionDto>> Handle(GetAllActionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Query the actions repository
                var actions = await _unitOfWork.Repository<OMSV1.Domain.Entities.Expenses.Action>()
                    .GetAllAsQueryable()
                    .Include(a => a.Profile) // Include Profile for mapping ProfileName
                    .Include(a => a.MonthlyExpenses) // Include MonthlyExpenses for completeness
                    .ProjectTo<ActionDto>(_mapper.ConfigurationProvider) // Map to ActionDto
                    .ToListAsync(cancellationToken);

                return actions;
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and throw a custom exception
                throw new HandlerException("An error occurred while retrieving actions.", ex);
            }
        }
    }
}

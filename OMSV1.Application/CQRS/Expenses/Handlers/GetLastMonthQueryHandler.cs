using AutoMapper;
using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Expenses;

namespace OMSV1.Application.Handlers.Expenses;

public class GetLastMonthQueryHandler : IRequestHandler<GetLastMonthQuery, List<MonthlyExpensesDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetLastMonthQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<MonthlyExpensesDto>> Handle(GetLastMonthQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var lastMonthSpec = new FilterLastMonthExpensesSpecification(
                request.OfficeId,
                request.Status
            );
            var lastMonthExpenses = await _unitOfWork.Repository<MonthlyExpenses>().ListAsync(lastMonthSpec);
            return _mapper.Map<List<MonthlyExpensesDto>>(lastMonthExpenses);
        }
        catch (Exception ex)
        {
            throw new HandlerException("An error occurred while retrieving last month's expenses.", ex);
        }
    }
}

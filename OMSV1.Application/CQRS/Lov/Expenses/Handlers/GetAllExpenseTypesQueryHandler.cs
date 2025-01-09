
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Expenses;
public class GetAllExpenseTypesQueryHandler : IRequestHandler<GetAllExpenseTypesQuery, PagedList<ExpenseTypeDto>>
{
    private readonly IGenericRepository<ExpenseType> _repository;
    private readonly IMapper _mapper;

    public GetAllExpenseTypesQueryHandler(IGenericRepository<ExpenseType> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedList<ExpenseTypeDto>> Handle(GetAllExpenseTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var expenseTypesQuery = _repository.GetAllAsQueryable();
            var mappedQuery = expenseTypesQuery.ProjectTo<ExpenseTypeDto>(_mapper.ConfigurationProvider);

            var pagedExpenseTypes = await PagedList<ExpenseTypeDto>.CreateAsync(
            mappedQuery,
            request.PaginationParams.PageNumber,
            request.PaginationParams.PageSize
            );

            return pagedExpenseTypes;
        }
        catch (Exception ex)
        {
            throw new HandlerException("An unexpected error occurred while retrieving all expense types.", ex);
        }
    }
}

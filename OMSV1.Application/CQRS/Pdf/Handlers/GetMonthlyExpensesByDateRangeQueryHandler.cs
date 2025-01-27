// using MediatR;
// using OMSV1.Application.Queries.Expenses;
// using OMSV1.Domain.Interfaces;
// using OMSV1.Infrastructure.Interfaces;

// namespace OMSV1.Application.Handlers.Expenses
// {
//     public class GetMonthlyExpensesByDateRangeQueryHandler : IRequestHandler<GetMonthlyExpensesByDateRangeQuery, string>
//     {
//         private readonly IMonthlyExpensesRepository _repository;
//         private readonly IPdfService _pdfService;

//         public GetMonthlyExpensesByDateRangeQueryHandler(IMonthlyExpensesRepository repository, IPdfService pdfService)
//         {
//             _repository = repository;
//             _pdfService = pdfService;
//         }

// public async Task<byte[]> Handle(GetMonthlyExpensesByDateRangeQuery request, CancellationToken cancellationToken)
// {
//     // Fetch domain entities within the specified date range
//     var monthlyExpenses = await _repository.GetExpensesByDateRangeAsync(request.StartDate, request.EndDate);

//     if (monthlyExpenses == null || !monthlyExpenses.Any())
//     {
//         throw new InvalidOperationException("No expenses found for the specified date range.");
//     }

//     // Generate the PDF and return as byte array
//     var pdfData = await _pdfService.GenerateMonthlyExpensesPdfAsync(monthlyExpenses);
//     return pdfData;
// }

//     }
// }

using MediatR;
using OMSV1.Application.Queries.Expenses;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.Handlers.Expenses
{
    public class GetMonthlyExpensesQueryHandler : IRequestHandler<GetMonthlyExpensesQuery, string>
    {
        private readonly IMonthlyExpensesRepository _repository;
        private readonly IPdfService _pdfService;

        public GetMonthlyExpensesQueryHandler(IMonthlyExpensesRepository repository, IPdfService pdfService)
        {
            _repository = repository;
            _pdfService = pdfService;
        }

        public async Task<string> Handle(GetMonthlyExpensesQuery request, CancellationToken cancellationToken)
        {
            // Fetch domain entities from the repository
            var monthlyExpenses = await _repository.GetAllMonthlyExpensesAsync();

            // Pass domain entities directly to the PDF service
            var pdfPath = await _pdfService.GenerateMonthlyExpensesPdfAsync(monthlyExpenses);
            return pdfPath;
        }
    }
}

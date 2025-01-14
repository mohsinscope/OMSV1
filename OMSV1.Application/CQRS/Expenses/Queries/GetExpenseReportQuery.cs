using MediatR;
using OMSV1.Application.DTOs.Expenses;

namespace OMSV1.Application.Queries.Expenses
{
    public class GetExpenseReportQuery : IRequest<ExpenseReportDto>
    {
        public Guid? GovernorateId { get; set; }
        public Guid? OfficeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public GetExpenseReportQuery(Guid? governorateId, Guid? officeId, DateTime? startDate, DateTime? endDate)
        {
            GovernorateId = governorateId;
            OfficeId = officeId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
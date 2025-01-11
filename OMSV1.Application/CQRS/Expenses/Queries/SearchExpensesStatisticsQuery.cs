using MediatR;
using OMSV1.Application.DTOs.Expenses;
using System;

namespace OMSV1.Application.Queries.Expenses
{
    public class SearchExpensesStatisticsQuery : IRequest<ExpensesStatisticsDto>
    {
        public Guid? OfficeId { get; set; }
        public Guid? GovernorateId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? ThresholdId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

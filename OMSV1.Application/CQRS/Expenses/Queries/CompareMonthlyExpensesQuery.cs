using MediatR;
using OMSV1.Application.DTOs.Expenses;
using System;

namespace OMSV1.Application.Queries.Expenses
{
    public class CompareMonthlyExpensesQuery : IRequest<ComparisonExpensesStatisticsDto>
    {
        public Guid? OfficeId { get; set; }
        public Guid? GovernorateId { get; set; }
        public Guid? ThresholdId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CompareMonthlyExpensesQuery(Guid? officeId, Guid? governorateId, Guid? thresholdId, DateTime startDate, DateTime endDate)
        {
            OfficeId = officeId;
            GovernorateId = governorateId;
            ThresholdId = thresholdId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

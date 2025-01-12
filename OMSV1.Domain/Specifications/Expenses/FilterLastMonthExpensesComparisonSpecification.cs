using System;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Expenses
{
    public class FilterLastMonthExpensesComparisonSpecification : BaseSpecification<MonthlyExpenses>
    {
        public FilterLastMonthExpensesComparisonSpecification(
            Guid? officeId = null,
            Guid? governorateId = null,
            Guid? thresholdId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
            : base(x =>
                (!officeId.HasValue || x.OfficeId == officeId) &&
                (!governorateId.HasValue || x.GovernorateId == governorateId) &&
                (!thresholdId.HasValue || x.ThresholdId == thresholdId) &&
                x.DateCreated >= startDate.Value.AddMonths(-1).Date && // Start of last month
                x.DateCreated < endDate.Value.AddMonths(-1).Date.AddDays(1).AddTicks(-1) && // End of last month inclusive
                x.Status == Status.Completed) // Only include completed expenses
        {
            // Include related entities
            AddInclude(x => x.Office);
            AddInclude(x => x.Governorate);
            AddInclude(x => x.Threshold);

            // Apply ordering by TotalAmount descending
            ApplyOrderByDescending(x => x.TotalAmount);
        }
    }
}
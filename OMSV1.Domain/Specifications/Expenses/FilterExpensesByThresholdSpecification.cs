using System;
using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Domain.Specifications.Expenses
{
    public class FilterExpensesByThresholdSpecification : BaseSpecification<MonthlyExpenses>
    {
        public FilterExpensesByThresholdSpecification(
            Guid? officeId = null,
            Guid? governorateId = null,
            Guid? thresholdId = null, // Filter by ThresholdId
            DateTime? startDate = null,
            DateTime? endDate = null)
            : base(x =>
                (!officeId.HasValue || x.OfficeId == officeId) &&
                (!governorateId.HasValue || x.GovernorateId == governorateId) &&
                (!thresholdId.HasValue || x.ThresholdId == thresholdId) && // Threshold filter
                (!startDate.HasValue || x.DateCreated >= startDate.Value) &&
                (!endDate.HasValue || x.DateCreated <= endDate.Value))
        {
            // Include related entities
            AddInclude(x => x.Office);
            AddInclude(x => x.Governorate);
            AddInclude(x => x.Threshold); // Include Threshold details

            // Apply ordering by Threshold.MaxValue (primary sort)
            ApplyOrderByDescending(x => x.TotalAmount );

           
            // Apply ordering by TotalAmount (secondary sort)
        }
    }
}

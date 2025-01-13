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
            x.DateCreated >= startDate.Value.Date &&
            x.DateCreated < endDate.Value.Date.AddDays(1) &&
            x.Status == Status.Completed) // Ensure only completed
    {
        AddInclude(x => x.Office);
        AddInclude(x => x.Governorate);
        AddInclude(x => x.Threshold);

        ApplyOrderByDescending(x => x.DateCreated);
    }
}

}
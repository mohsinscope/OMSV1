using System;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Expenses
{
public class FilterExpensesSpecification : BaseSpecification<MonthlyExpenses>
{
    public FilterExpensesSpecification(
        Guid? officeId = null,
        Guid? governorateId = null,
        Guid? profileId = null,
        ICollection<Status>? statuses = null, // Changed from Status? to ICollection<Status>?
        DateTime? startDate = null,
        DateTime? endDate = null
        )
        : base(x =>
            (!officeId.HasValue || x.OfficeId == officeId) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId) &&
            (!profileId.HasValue || x.ProfileId == profileId) &&
            (statuses == null || !statuses.Any() || statuses.Contains(x.Status)) && // Updated status filter
            (!startDate.HasValue || x.DateCreated >= startDate.Value) &&
            (!endDate.HasValue || x.DateCreated <= endDate.Value))
    {
        AddInclude(x => x.Office);
        AddInclude(x => x.Governorate);
        AddInclude(x => x.Profile);
        ApplyOrderByDescending(x => x.DateCreated);
    }
}
}

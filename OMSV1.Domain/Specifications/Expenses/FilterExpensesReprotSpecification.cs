using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Specifications;
using System;

namespace OMSV1.Domain.Specifications.Expenses
{
    public class FilterExpensesReprotSpecification : BaseSpecification<MonthlyExpenses>
    {
        public FilterExpensesReprotSpecification(
            Guid? governorateId = null,
            Guid? officeId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
            : base(x =>
                (!governorateId.HasValue || x.GovernorateId == governorateId) &&
                (!officeId.HasValue || x.OfficeId == officeId) &&
                (!startDate.HasValue || x.DateCreated >= startDate.Value) &&
                (!endDate.HasValue || x.DateCreated <= endDate.Value))
        {
        }
    }
}
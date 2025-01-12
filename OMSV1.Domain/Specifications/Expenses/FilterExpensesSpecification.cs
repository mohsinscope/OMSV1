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
            Status? status = null, // Enum Status
            DateTime? startDate = null,
            DateTime? endDate = null,
            int pageNumber = 1,
            int pageSize = 10)
            : base(x =>
                (!officeId.HasValue || x.OfficeId == officeId) &&
                (!governorateId.HasValue || x.GovernorateId == governorateId) &&
                (!profileId.HasValue || x.ProfileId == profileId) &&
                (!status.HasValue || x.Status == status) && // Enum comparison
                (!startDate.HasValue || x.DateCreated >= startDate.Value) &&
                (!endDate.HasValue || x.DateCreated <= endDate.Value))
        {
            // Include related entities
            AddInclude(x => x.Office);
            AddInclude(x => x.Governorate);
            AddInclude(x => x.Profile);

            // Apply ordering and pagination
            ApplyOrderByDescending(x => x.DateCreated);
            //ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }
    }
}

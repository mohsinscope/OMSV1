using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Expenses;
public class FilterLastMonthExpensesSpecification : BaseSpecification<MonthlyExpenses>
{
    public FilterLastMonthExpensesSpecification(Guid? officeId,Status? status)
        : base(x =>
            (!officeId.HasValue || x.OfficeId == officeId) &&
            (status == null || (x.Status != Status.New && x.Status != Status.Completed)))// Exclude New and Completed statuses
    {
        AddInclude(x => x.Office);
        AddInclude(x => x.Threshold);
        AddInclude(x => x.Governorate);
        AddInclude(x => x.Profile);


    }
}

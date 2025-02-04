using System;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Attendances
{
public class FilterAttendanceunavailableStatisticsSpecification : BaseSpecification<Attendance>
{
    public FilterAttendanceunavailableStatisticsSpecification(
        DateTime? date = null,
        int? workingHours = null,
        Guid? governorateId = null)
        : base(x =>
            (!date.HasValue || x.Date.Date == date.Value.Date) && // Compare only dates
            (!workingHours.HasValue || 
             workingHours.Value == (int)WorkingHours.Both || 
             x.WorkingHours == (WorkingHours)workingHours.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value))
    {
        AddInclude(x => x.Office);
        ApplyOrderBy(x => x.Office.Code);

    }
}

}

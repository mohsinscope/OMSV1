using System;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Attendances
{
    public class FilterAttendanceInOfficesStatisticsSpecification : BaseSpecification<Attendance>
    {
   public FilterAttendanceInOfficesStatisticsSpecification(
    int? workingHours = null,
    DateTime? date = null,
    Guid? officeId = null)
    : base(x =>
        (workingHours == null || 
        (workingHours.Value == (int)WorkingHours.Both) || 
        x.WorkingHours == (WorkingHours)workingHours.Value) &&
        (!date.HasValue || x.Date == date.Value) && // Handle nullable date
        (!officeId.HasValue || x.OfficeId == officeId.Value))
{
    // Include only necessary relationships
    AddInclude(x => x.Office); // Compiler now knows Office is initialized

    // Apply ordering
    ApplyOrderByDescending(x => x.Date);
}
    }
}

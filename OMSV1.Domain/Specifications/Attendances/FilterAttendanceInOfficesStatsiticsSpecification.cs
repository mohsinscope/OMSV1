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
            int? officeId = null)
            : base(x =>
                (workingHours == null || 
                (workingHours.Value == (int)WorkingHours.Both) || 
                x.WorkingHours == (WorkingHours)workingHours.Value) &&

                (x.Date == date)&&  // Match the exact date
                (!officeId.HasValue || x.OfficeId == officeId.Value))
        {
            // Include only necessary relationships
            AddInclude(x => x.Office);

            // Apply ordering
            ApplyOrderByDescending(x => x.Date);
        }
    }
}

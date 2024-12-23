using System;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Attendances
{
    public class FilterAttendanceStatisticsSpecification : BaseSpecification<Attendance>
    {
        public FilterAttendanceStatisticsSpecification(
            int? workingHours = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? officeId = null,
            int? governorateId = null)
            : base(x =>
                (workingHours == null || 
                (workingHours.Value == (int)WorkingHours.Both) || 
                x.WorkingHours == (WorkingHours)workingHours.Value) &&

                (!startDate.HasValue || x.Date >= startDate.Value) &&
                (!endDate.HasValue || x.Date <= endDate.Value) &&
                (!officeId.HasValue || x.OfficeId == officeId.Value) &&
                (!governorateId.HasValue || x.GovernorateId == governorateId.Value))
        {
            // Include only necessary relationships
            AddInclude(x => x.Office);
            AddInclude(x => x.Governorate);

            // Apply ordering
            ApplyOrderByDescending(x => x.Date);
        }
    }
}

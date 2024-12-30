using System;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Attendances;

public class FilterAttendanceSpecification : BaseSpecification<Attendance>
{
    public FilterAttendanceSpecification(
        int? workingHours = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? officeId = null,
        int? governorateId = null,
        int? profileId = null)
        : base(x =>
        
            (workingHours == null || 
            (workingHours.Value == (int)WorkingHours.Both) ||
            x.WorkingHours == (WorkingHours)workingHours.Value) &&

            (!startDate.HasValue || x.Date >= startDate.Value) &&
            (!endDate.HasValue || x.Date <= endDate.Value) &&
            (!officeId.HasValue || x.OfficeId == officeId.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
            (!profileId.HasValue || x.ProfileId == profileId.Value))
    {
        AddInclude(x => x.Governorate);
        AddInclude(x => x.Office);
        AddInclude(x => x.Profile);
                   // Apply ordering
         ApplyOrderByDescending(x => x.DateCreated);
    }
}

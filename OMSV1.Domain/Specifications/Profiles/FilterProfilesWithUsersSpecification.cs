using System;
using OMSV1.Domain.Entities.Profiles;

namespace OMSV1.Domain.Specifications.Profiles;

public class FilterProfilesWithUsersSpecification : BaseSpecification<OMSV1.Domain.Entities.Profiles.Profile>
{
    public FilterProfilesWithUsersSpecification(
        string? fullName = null,
        Guid? officeId = null,
        Guid? governorateId = null)
        : base(x =>
            (string.IsNullOrEmpty(fullName) || x.FullName.Contains(fullName)) &&
            (!officeId.HasValue || x.OfficeId == officeId.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value))
    {
        AddInclude(x => x.Governorate);
        AddInclude(x => x.Office);

           // Apply ordering
         ApplyOrderByDescending(x => x.DateCreated);
    }
}

using OMSV1.Domain.Entities;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Specifications;

namespace OMSV1.Domain.Specifications.Profiles
{
    public class FilterProfilesSpecification : BaseSpecification<Profile>
    {
        public FilterProfilesSpecification(string fullName, int? officeId, int? governorateId)
            : base(profile =>
                (string.IsNullOrEmpty(fullName) || profile.FullName.Contains(fullName)) &&
                (!officeId.HasValue || profile.OfficeId == officeId) &&
                (!governorateId.HasValue || profile.GovernorateId == governorateId))
        {
            AddInclude(profile => profile.Office);
            AddInclude(profile => profile.Governorate);
        }
    }
}

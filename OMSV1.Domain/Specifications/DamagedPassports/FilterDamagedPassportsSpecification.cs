using System;
using OMSV1.Domain.Entities.DamagedPassport;

namespace OMSV1.Domain.Specifications.DamagedPassports;

public class FilterDamagedPassportsSpecification : BaseSpecification<DamagedPassport>
{
    public FilterDamagedPassportsSpecification(
        string? passportNumber = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        Guid? damagedTypeId = null,
        Guid? officeId = null,
        Guid? governorateId = null,
        Guid? profileId = null)
        : base(x =>
            (string.IsNullOrEmpty(passportNumber) || x.PassportNumber.Contains(passportNumber)) &&
            (!startDate.HasValue || x.Date >= startDate.Value) &&
            (!endDate.HasValue || x.Date <= endDate.Value) &&
            (!damagedTypeId.HasValue || x.DamagedTypeId == damagedTypeId.Value) &&
            (!officeId.HasValue || x.OfficeId == officeId.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
            (!profileId.HasValue || x.ProfileId == profileId.Value))
    {
        AddInclude(x => x.Governorate);
        AddInclude(x => x.DamagedType);
        AddInclude(x => x.Office);
        AddInclude(x => x.Profile);

           // Apply ordering
         ApplyOrderByDescending(x => x.Date);
    }
}

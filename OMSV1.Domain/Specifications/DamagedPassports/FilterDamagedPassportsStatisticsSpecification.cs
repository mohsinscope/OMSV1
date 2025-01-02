using System;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Specifications.DamagedPassports
{
public class FilterDamagedPassportsStatisticsSpecification : BaseSpecification<DamagedPassport>
{
    public FilterDamagedPassportsStatisticsSpecification(
        Guid? officeId = null,
        Guid? governorateId = null,
        Guid? damagedTypeId = null,
        DateTime? date = null)
        : base(x =>
            (!officeId.HasValue || x.OfficeId == officeId.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
            (!damagedTypeId.HasValue || x.DamagedTypeId == damagedTypeId.Value) &&
            (x.Date == date)  // Match the exact date
        )
    {
        // Include related entities (if necessary)
        AddInclude(x => x.Office);  
        AddInclude(x => x.Governorate); 

        // Apply ordering
        ApplyOrderByDescending(x => x.Date);
    }
}

}

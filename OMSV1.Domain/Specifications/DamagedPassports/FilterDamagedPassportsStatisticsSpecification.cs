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
        DateTime? startDate = null,
        DateTime? endDate = null)
        : base(x =>
            (!officeId.HasValue || x.OfficeId == officeId.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
            (!damagedTypeId.HasValue || x.DamagedTypeId == damagedTypeId.Value) &&
            (!startDate.HasValue || x.Date >= startDate.Value) &&
            (!endDate.HasValue || x.Date <= endDate.Value)        )
    {
        // Include related entities (if necessary)
        AddInclude(x => x.Office);  
        AddInclude(x => x.Governorate); 

    }
}
}
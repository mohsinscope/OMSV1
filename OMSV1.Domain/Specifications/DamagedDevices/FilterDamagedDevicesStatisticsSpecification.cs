using System;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Specifications.DamagedDevices
{
public class FilterDamagedDevicesStatisticsSpecification : BaseSpecification<DamagedDevice>
{
    public FilterDamagedDevicesStatisticsSpecification(
        int? officeId = null,
        int? governorateId = null,
        int? damagedDeviceTypeId = null,
        DateTime? date = null)
        : base(x =>
            (!officeId.HasValue || x.OfficeId == officeId.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
            (!damagedDeviceTypeId.HasValue || x.DamagedDeviceTypeId == damagedDeviceTypeId.Value) &&
            (!date.HasValue || x.Date == date.Value) // Show all data if date is null
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

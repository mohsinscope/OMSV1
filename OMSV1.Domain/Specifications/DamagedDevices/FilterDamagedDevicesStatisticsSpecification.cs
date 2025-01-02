using System;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Specifications.DamagedDevices
{
public class FilterDamagedDevicesStatisticsSpecification : BaseSpecification<DamagedDevice>
{
    public FilterDamagedDevicesStatisticsSpecification(
        Guid? officeId = null,
        Guid? governorateId = null,
        Guid? damagedDeviceTypeId = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
        : base(x =>
            (!officeId.HasValue || x.OfficeId == officeId.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
            (!damagedDeviceTypeId.HasValue || x.DamagedDeviceTypeId == damagedDeviceTypeId.Value) &&
            (!startDate.HasValue || x.Date >= startDate.Value) &&
            (!endDate.HasValue || x.Date <= endDate.Value)        )
    {
        // Include related entities (if necessary)
        AddInclude(x => x.Office);  
        AddInclude(x => x.Governorate); 

        }
    }
}

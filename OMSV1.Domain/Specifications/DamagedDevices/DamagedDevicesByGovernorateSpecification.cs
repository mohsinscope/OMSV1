using System;
using OMSV1.Domain.Entities.DamagedDevices;

namespace OMSV1.Domain.Specifications.DamagedDevices;

public class DamagedDevicesByGovernorateSpecification : BaseSpecification<DamagedDevice>
{
    public DamagedDevicesByGovernorateSpecification(
        Guid governorateId, 
        DateTime? startDate = null, 
        DateTime? endDate = null, 
        Guid? deviceTypeId = null, 
        int pageNumber = 1, 
        int pageSize = 10) 
        : base(x => 
            x.GovernorateId == governorateId && 
            (!startDate.HasValue || x.Date >= startDate.Value) && 
            (!endDate.HasValue || x.Date <= endDate.Value) && 
            (!deviceTypeId.HasValue || x.DeviceTypeId == deviceTypeId.Value))
    {
        AddInclude(x => x.Governorate);
        AddInclude(x => x.DeviceType);
        AddInclude(x => x.DamagedDeviceTypes);
        
        ApplyOrderByDescending(x => x.Date);
        ApplyPaging((pageNumber - 1) * pageSize, pageSize);
    }
}
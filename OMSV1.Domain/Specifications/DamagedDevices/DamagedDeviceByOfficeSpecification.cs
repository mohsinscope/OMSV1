using System;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Offices;

namespace OMSV1.Domain.Specifications.DamagedDevices
{
    public class DamagedDevicesByOfficeSpecification : BaseSpecification<DamagedDevice>
    {
        public DamagedDevicesByOfficeSpecification(
            Guid officeId, 
            DateTime? startDate = null, 
            DateTime? endDate = null, 
            int pageNumber = 1, 
            int pageSize = 10) 
            : base(x => 
                x.OfficeId == officeId && 
                (!startDate.HasValue || x.Date >= startDate.Value) && 
                (!endDate.HasValue || x.Date <= endDate.Value))
        {
            // Include related entities
            AddInclude(x => x.Office);
            AddInclude(x => x.DeviceType);
            AddInclude(x => x.Governorate);
            AddInclude(x => x.Profile);
            AddInclude(x => x.DamagedDeviceTypes);

            // Apply ordering and pagination
            ApplyOrderByDescending(x => x.Date);
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }
    }
}

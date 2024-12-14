using System;
using OMSV1.Domain.Entities.DamagedDevices;

namespace OMSV1.Domain.Specifications.DamagedDevices;

public class DamagedDeviceBySerialNumberSpecification : BaseSpecification<DamagedDevice>
{
    public DamagedDeviceBySerialNumberSpecification(string serialNumber) 
        : base(x => x.SerialNumber == serialNumber)
    {
        AddInclude(x => x.DamagedDeviceTypes);
        AddInclude(x => x.DeviceType);
        AddInclude(x => x.Office);
    }
}
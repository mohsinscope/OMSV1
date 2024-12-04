using System;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Attachments;

public class AttachmentCU(string name, string url, int damagedDeviceId) :Entity
{

    public string Name { get; private set; } = name;
    public string Url { get; private set; } = url;
    public int DamagedDeviceId { get; private set; } = damagedDeviceId;
    public DamagedDevice? DamagedDevice { get; private set; }

}
 
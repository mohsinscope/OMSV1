using System;
using System.Net.Mail;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedDevices;

public class DamagedDevice(
                             string serialNumber,
                             DateTime date,
                             int damagedDeviceTypeId,
                             int deviceTypeId,
                             int officeId,
                             int governorateId,
                             int profileId) : Entity
{
    public string SerialNumber { get; private set; } = serialNumber;
    public DateTime Date { get; private set; } = date;
    public int DamagedDeviceTypeId { get; private set; } = damagedDeviceTypeId;
    public int DeviceTypeId { get; private set; } = deviceTypeId;
    public int OfficeId { get; private set; } = officeId;
    public int GovernorateId { get; private set; } = governorateId;
    public int ProfileId { get; private set; } = profileId;
    public DamagedDeviceType? DamagedType { get; private set; }
    public DeviceType? DeviceType { get; private set; }
    public Governorate? Governorate { get; private set; }
    public Office? Office { get; private set; }

    // protected List<AttachmentCU> _attachments = new List<AttachmentCU>();
    // public IReadOnlyCollection<OMSV1.Domain.Entities.Attachments.AttachmentCU> AttachmentCUs => _attachments.AsReadOnly();


}


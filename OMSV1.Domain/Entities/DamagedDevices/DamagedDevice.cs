using System;
using System.Net.Mail;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Entities.Profiles;

namespace OMSV1.Domain.Entities.DamagedDevices;

public class DamagedDevice(
                             string serialNumber,
                             DateTime date,
                             int damagedDeviceTypeId,
                             string note,
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
    public DamagedDeviceType? DamagedDeviceTypes { get; private set; }
    public DeviceType? DeviceType { get; private set; }
    public string Note { get; private set; } = note;
    public Governorate? Governorate { get; private set; }
    public Office? Office { get; private set; }
    public Profile? Profile { get; private set; }



        // Method to update the device details
    public void UpdateDeviceDetails(
        string serialNumber,
        DateTime date,
        int damagedDeviceTypeId,
        int deviceTypeId,
        int officeId,
        string note,
        int governorateId,
        int profileId)
    {
        SerialNumber = serialNumber;
        Date = date;
        DamagedDeviceTypeId = damagedDeviceTypeId;
        DeviceTypeId = deviceTypeId;
        OfficeId = officeId;
        Note = note;
        GovernorateId = governorateId;
        ProfileId = profileId;
    }
            public DamagedDevice(string serialNumber, DateTime date)
            : this(serialNumber, date, default, default, default, default, default,default)
        {
        }

     public void UpdateDate(DateTime date)
    {
        Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }
}


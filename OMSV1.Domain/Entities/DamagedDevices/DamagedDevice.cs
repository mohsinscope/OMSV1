using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedDevices
{
    public class DamagedDevice : Entity
    {
        public string SerialNumber { get; private set; }
        public DateTime Date { get; private set; }
        public Guid DamagedDeviceTypeId { get; private set; }
        public Guid DeviceTypeId { get; private set; }
        public Guid OfficeId { get; private set; }
        public Guid GovernorateId { get; private set; }
        public Guid ProfileId { get; private set; }
        public string? Note { get; private set; }
        
        // Navigation properties (optional nullability)
        public DamagedDeviceType? DamagedDeviceTypes { get; private set; }
        public DeviceType? DeviceType { get; private set; }
        public Governorate? Governorate { get; private set; }
        public Office? Office { get; private set; }
        public Profile? Profile { get; private set; }

        // Constructor for full initialization
        public DamagedDevice(string serialNumber, DateTime date, Guid damagedDeviceTypeId, string note,
                             Guid deviceTypeId, Guid officeId, Guid governorateId, Guid profileId) : base()
        {
            SerialNumber = serialNumber;
            Date = date;
            DamagedDeviceTypeId = damagedDeviceTypeId;
            DeviceTypeId = deviceTypeId;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            Note = note;
        }

        // Constructor with default values (optional parameters)
        public DamagedDevice(string serialNumber, DateTime date)
            : this(serialNumber, date, default, default, default, default,default,default)
        {
        }
        // Method to update the device details
        public void UpdateDeviceDetails(string serialNumber, DateTime date, Guid damagedDeviceTypeId, Guid deviceTypeId,
                                        Guid officeId, string note, Guid governorateId, Guid profileId)
        {
            if (string.IsNullOrEmpty(serialNumber)) throw new ArgumentException("Serial number cannot be null or empty", nameof(serialNumber));

            SerialNumber = serialNumber;
            Date = date;
            DamagedDeviceTypeId = damagedDeviceTypeId;
            DeviceTypeId = deviceTypeId;
            OfficeId = officeId;
            Note = note;
            GovernorateId = governorateId;
            ProfileId = profileId;
        }

        // Method to update the Date (ensure UTC Kind)
        public void UpdateDate(DateTime date)
        {
            Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }
    }
}

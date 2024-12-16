using System;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedPassport;

public class DamagedPassport : Entity
{
    public string PassportNumber { get; private set; }
    public DateTime Date { get; private set; }
    public int DamagedTypeId { get; private set; }
    public int OfficeId { get; private set; }
    public int GovernorateId { get; private set; }
    public int ProfileId { get; private set; }

    public Governorate? Governorate { get; private set; }
    public Profile? Profile { get; private set; }
    public Office? Office { get; private set; }
    public DamagedType? DamagedType { get; private set; }

    private readonly List<AttachmentCU> _attachments = new();
    public IReadOnlyCollection<AttachmentCU> Attachments => _attachments.AsReadOnly();

    // Main constructor that handles all properties
    public DamagedPassport(string passportNumber, DateTime date, 
                            int officeId, int governorateId, 
                            int damagedTypeId, int profileId) : base()
    {
        PassportNumber = passportNumber;
        Date = date;
        OfficeId = officeId;
        GovernorateId = governorateId;
        DamagedTypeId = damagedTypeId;
        ProfileId = profileId;
    }

    // Constructor with default values (optional parameters)
    public DamagedPassport(string passportNumber, DateTime date)
        : this(passportNumber, date, default, default, default, default)
    {
    }

  public void UpdateDate(DateTime date)
{
    // Ensure the Date is set with UTC Kind
    Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
}

}

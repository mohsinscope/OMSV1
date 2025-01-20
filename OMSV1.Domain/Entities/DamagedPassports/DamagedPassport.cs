using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedPassport;

public class DamagedPassport : Entity
{
    public new Guid Id { get; set; }
    public string PassportNumber { get; private set; }
    public DateTime Date { get; private set; }
    public Guid DamagedTypeId { get; private set; }
    public string Note { get; private set; }

    public Guid OfficeId { get; private set; }
    public Guid GovernorateId { get; private set; }
    public Guid ProfileId { get; private set; }

    public Governorate Governorate { get; private set; }= null!;
    public Profile Profile { get; private set; }= null!;
    public Office Office { get; private set; }= null!;
    public DamagedType DamagedType { get; private set; }= null!;

    // Main constructor that handles all properties
    public DamagedPassport(string passportNumber, DateTime date, string note,
                            Guid officeId, Guid governorateId, 
                            Guid damagedTypeId, Guid profileId) : base()
    {
        PassportNumber = passportNumber;
        Date = date;
        Note = note;
        OfficeId = officeId;
        GovernorateId = governorateId;
        DamagedTypeId = damagedTypeId;
        ProfileId = profileId;
    }

        // Constructor with default values (optional parameters)
        public DamagedPassport(string passportNumber, DateTime date)
            : this(passportNumber, date, string.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty)
        {
        }
        public void UpdateDeviceDetails(
            string passportNumber,
            DateTime date,
            Guid damagedTypeId,
            string note,
            Guid officeId,
            Guid governorateId,
            Guid profileId)
        {
            // Update the properties of the DamagedPassport entity with the new values
            PassportNumber = passportNumber;
            Date = date;
            DamagedTypeId = damagedTypeId;
            Note = note;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
        }
    public void UpdateDate(DateTime date)
    {
        // Ensure the Date is set with UTC Kind
        Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }

}

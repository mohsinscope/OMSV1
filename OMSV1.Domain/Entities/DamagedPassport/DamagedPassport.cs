using System;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedPassport;

public class DamagedPassport(string passportNumber,
                             DateTime date,
                             int officeId,
                             int governorateId,
                             //  int profileId,
                             int damagedTypeId,
                             int profileId) : Entity
{
    public string PassportNumber { get; private set; } = passportNumber;
    public DateTime Date { get; private set; } = date;
    public int DamagedTypeId { get; private set; } = damagedTypeId;
    public int OfficeId { get; private set; } = officeId;
    public int GovernorateId { get; private set; } = governorateId;
    public int ProfileId { get; private set; } = profileId;

    public Governorate? Governorate { get; private set; }
    public Profile? Profile { get; private set; }

    public Office? Office { get; private set; }
    public DamagedType? DamagedType { get; private set; }


    private readonly List<AttachmentCU> _attachments = new();
    public IReadOnlyCollection<AttachmentCU> Attachments => _attachments.AsReadOnly();




    public void AddAttachment(string fileName, string filePath)
    {
        var attachment = new AttachmentCU(
            fileName: fileName,
            filePath: filePath,
            entityType: EntityType.DamagedPassport,
            entityId: Id
        );

        _attachments.Add(attachment);
    }
}

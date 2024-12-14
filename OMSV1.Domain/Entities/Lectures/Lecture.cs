using System;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Lectures;

public class Lecture(string title,
                     DateTime date,
                     int officeId,
                     int governorateId,
                     int profileId) : Entity
{
    public string Title { get; private set; } = title;
    public DateTime Date { get; private set; } = date;
    public int OfficeId { get; private set; } = officeId;
    public int GovernorateId { get; private set; } = governorateId;
    public int ProfileId { get; private set; } = profileId;
    public Governorate? Governorate { get; private set; }
    public Office? Office { get; private set; }
    public Profile? Profile { get; private set; }


    private readonly List<AttachmentCU> _attachments = new();
    public IReadOnlyCollection<AttachmentCU> Attachments => _attachments.AsReadOnly();





}

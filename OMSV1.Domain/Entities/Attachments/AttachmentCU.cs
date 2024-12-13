using System;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Entities.Lectures;

namespace OMSV1.Domain.Entities.Attachments;

public class AttachmentCU(string fileName, string filePath, EntityType entityType, int entityId, int? damagedDeviceId=null, int? lectureId=null) : Entity
{

    public string FileName { get; private set; } = fileName;
    public string FilePath { get; private set; } = filePath;
    public EntityType EntityType { get; private set; } = entityType;
    public int EntityId { get; private set; } = entityId;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public int? DamagedDeviceId { get; private set; } = damagedDeviceId;
    public int? LectureId { get; private set; } = lectureId;
    public DamagedDevice? DamagedDevice { get; private set; }
    public Lecture? Lecture { get; private set; }
}
 
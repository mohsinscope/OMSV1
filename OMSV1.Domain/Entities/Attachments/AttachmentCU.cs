using System;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Entities.Lectures;

namespace OMSV1.Domain.Entities.Attachments;

public class AttachmentCU(string fileName, string filePath, EntityType entityType, int entityId) : Entity
{
    public int Id { get; set; }
    public string FileName { get; private set; } = fileName;
    public string FilePath { get; private set; } = filePath;
    public EntityType EntityType { get; private set; } = entityType;
    public int EntityId { get; private set; } = entityId;

}
 
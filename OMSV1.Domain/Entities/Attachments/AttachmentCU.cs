using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Entities.Attachments;

public class AttachmentCU : Entity
{
    public int Id { get; set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public EntityType EntityType { get; private set; }
    public int EntityId { get; private set; }

    public AttachmentCU(string fileName, string filePath, EntityType entityType, int entityId)
    {
        FileName = fileName;
        FilePath = filePath;
        EntityType = entityType;
        EntityId = entityId;
    }

    public void Update(string fileName, string filePath, EntityType entityType, int entityId)
    {
        FileName = fileName;
        FilePath = filePath;
        EntityType = entityType;
        EntityId = entityId;
    }
}

using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Entities.Attachments;

public class AttachmentCU(string fileName, string filePath, EntityType entityType, int entityId) : Entity
{
    public int Id { get; set; }
    public string FileName { get; private set; } = fileName;
    public string FilePath { get; private set; } = filePath;
    public EntityType EntityType { get; private set; } = entityType;
    public int EntityId { get; private set; } = entityId;
           // Constructor to initialize properties

        // Update method to modify properties
        public void Update(string fileName, string filePath, EntityType entityType, int entityId)
        {
            FileName = fileName;
            FilePath = filePath;
            EntityType = entityType;
            EntityId = entityId;
        }
                // Method to update the file path (for when the file is replaced)
        public void UpdateFilePath(string filePath)
        {
            FilePath = filePath;
        }


}
 
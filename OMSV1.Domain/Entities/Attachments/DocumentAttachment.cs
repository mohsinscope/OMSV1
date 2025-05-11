using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Attachments
{
    /// <summary>
    /// مرفق خاص بالوثائق فقط (EntityType = Document دائماً)
    /// </summary>
    public class DocumentAttachment : Entity
    {
        /* ─────────────── Scalar fields ─────────────── */
        public string     FilePath   { get; private set; }
        public Guid       DocumentId { get; private set; }

        /// <summary>
        /// يظل ثابتاً على Document لضمان الفصل عن AttachmentCU
        /// </summary>
        public EntityType EntityType { get; private set; } = EntityType.Document;

        /* ─────────────── Navigation properties ─────────────── */
        public Document Document { get; private set; } = null!;

        /* ─────────────── Constructors ─────────────── */
        // For EF
        private DocumentAttachment() { /* EF */ }

        // For creating a new attachment
        public DocumentAttachment(string filePath, Guid documentId)
        {
            FilePath   = filePath;
            DocumentId = documentId;
            EntityType = EntityType.Document;   // الدفاعية: تظل ثابتة
        }

        /* ─────────────── Behaviours ─────────────── */
        public void UpdateFilePath(string newPath) => FilePath = newPath;
    }
}
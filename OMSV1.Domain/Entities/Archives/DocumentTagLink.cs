// Domain/Entities/Documents/DocumentTagLink.cs
namespace OMSV1.Domain.Entities.Documents
{
    public class DocumentTagLink   // no Entity base → composite PK
    {
        public Guid DocumentId { get; private set; }
        public Document Document { get; private set; } = null!;

        public Guid TagId      { get; private set; }
        public Tag Tag         { get; private set; } = null!;

        protected DocumentTagLink() { }  // EF

        public DocumentTagLink(Guid documentId, Guid tagId)
        {
            if (documentId == Guid.Empty || tagId == Guid.Empty)
                throw new ArgumentException("Ids must be valid GUIDs.");

            DocumentId = documentId;
            TagId      = tagId;
        }
    }
}

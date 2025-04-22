// Domain/Entities/Documents/DocumentCcLink.cs
namespace OMSV1.Domain.Entities.Documents;

public class DocumentCcLink   // no Entity base → composite PK
{
    public Guid DocumentId     { get; private set; }
    public Document Document   { get; private set; } = null!;

    public Guid DocumentCcId   { get; private set; }
    public DocumentCC DocumentCc { get; private set; } = null!;

    protected DocumentCcLink() { }  // EF

    public DocumentCcLink(Guid documentId, Guid documentCcId)
    {
        if (documentId == Guid.Empty || documentCcId == Guid.Empty)
            throw new ArgumentException("Ids must be valid GUIDs.");
        DocumentId     = documentId;
        DocumentCcId   = documentCcId;
    }
}

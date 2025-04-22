// Domain/Entities/Documents/DocumentCC.cs
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Documents;

public class DocumentCC : Entity
{
    public string RecipientName { get; private set; } = string.Empty;

    // navigation to join‑table
    private readonly List<DocumentCcLink> _documentLinks = new();
    public IReadOnlyCollection<DocumentCcLink> DocumentLinks => _documentLinks.AsReadOnly();

    protected DocumentCC() { }          // for EF / serialization

    public DocumentCC(string recipientName)
    {
        if (string.IsNullOrWhiteSpace(recipientName))
            throw new ArgumentException("RecipientName cannot be empty.", nameof(recipientName));

        RecipientName = recipientName.Trim();
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("RecipientName cannot be empty.", nameof(name));

        RecipientName = name.Trim();
    }
}

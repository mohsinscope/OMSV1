using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Documents
{
    /// <summary>
    /// A Tag that can be associated with many Documents.
    /// </summary>
// Domain/Entities/Documents/Tag.cs
public class Tag : Entity
{
    public string Name { get; private set; } = string.Empty;

    // ← NEW: link rows (no direct `Documents` collection any more)
    private readonly List<DocumentTagLink> _tagLinks = new();
    public IReadOnlyCollection<DocumentTagLink> TagLinks 
        => _tagLinks.AsReadOnly();

    protected Tag() { }

    public Tag(string name) : this()
    {
        Name = name;
    }

    public void UpdateName(string name) { /*...*/ }
}

}

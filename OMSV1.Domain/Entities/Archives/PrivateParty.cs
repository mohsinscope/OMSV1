// Domain/Entities/Documents/PrivateParty.cs
using System.Collections.Generic;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Documents
{
    /// <summary>
    /// An optional private party reference for documents.
    /// </summary>
    public class PrivateParty : Entity
    {
        public string Name { get; private set; } = string.Empty;

        // Documents may optionally reference this party
        private readonly List<Document> _documents = new();
        public IReadOnlyCollection<Document> Documents => _documents;

        protected PrivateParty() { }

        public PrivateParty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            Name = name;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            Name = name;
        }
    }
}

using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace OMSV1.Domain.Entities.Projects
{
    public class Project : Entity
    {
        public string Name { get; private set; }

        // One project has many documents
        public ICollection<Document> Documents { get; private set; }

        // One project has many parties
        public ICollection<Documents.DocumentParty> Parties { get; private set; }

        // EF / Serialization constructor
        protected Project()
        {
            Name = string.Empty;
            Documents = new List<Document>();
            Parties = new List<Documents.DocumentParty>();
        }

        public Project(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            Name = name;
            Documents = new List<Document>();
            Parties = new List<Documents.DocumentParty>();
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be null or empty.", nameof(newName));

            Name = newName;
        }

        // Document methods
        public void AddDocument(Document document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            Documents.Add(document);
        }

        // Party methods
        public void AddParty(Documents.DocumentParty party)
        {
            if (party == null) throw new ArgumentNullException(nameof(party));
            Parties.Add(party);
        }
    }
}

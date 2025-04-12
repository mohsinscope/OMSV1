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

        // EF / Serialization constructor
        protected Project()
        {
            Name = string.Empty;
            Documents = new List<Document>();
        }

        public Project(string name)
        {
            Name = name;
            Documents = new List<Document>();
        }
        // In OMSV1.Domain.Entities.Projects.Project
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be null or empty.", nameof(newName));

            Name = newName;
        }


        // Example domain method
        public void AddDocument(Document document)
        {
            // Possibly add some business rules or checks
            Documents.Add(document);
        }
    }
}

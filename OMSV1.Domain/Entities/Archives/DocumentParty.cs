using OMSV1.Domain.SeedWork;
using System;

namespace OMSV1.Domain.Entities.Documents
{

    public class DocumentParty : Entity
    {
        // Set a default empty string to guarantee a non-null value.
        public string Name { get; private set; } = string.Empty;
        // EF / Serialization constructor
        protected DocumentParty() { }

        // Main constructor
        public DocumentParty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            
            Name = name;
        }
        public void UpdateName(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be null or empty.", nameof(name));
                
                Name = name;
            }

    }
}

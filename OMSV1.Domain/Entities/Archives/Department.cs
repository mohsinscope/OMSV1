// Ministry.cs
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Sections
{
    /// <summary>
    /// Represents a government ministry associated with a document.
    /// </summary>
    public class Department : Entity
    {
        /// <summary>
        /// The name of the ministry.
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        public Guid DirectorateId { get; private set; }
        public Directorate Directorate { get; private set; } = null!;


        /// <summary>
        /// Documents optionally linked to this ministry.
        /// </summary>
        // public ICollection<OMSV1.Domain.Entities.Documents.Document> Documents { get; private set; }
        public ICollection<Section> Sections { get; private set; }


        protected Department()
        {
            Sections = new List<Section>();

            // Documents = new List<OMSV1.Domain.Entities.Documents.Document>();
        }
        public Department(string name, Guid directorateId) : this()
        {
            Name = name;
            DirectorateId = directorateId;
        }

      public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            Name = name;
        }
    }
}

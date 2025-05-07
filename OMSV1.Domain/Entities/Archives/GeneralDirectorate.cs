// Ministry.cs
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.GeneralDirectorates
{
    /// <summary>
    /// Represents a government ministry associated with a document.
    /// </summary>
    public class GeneralDirectorate : Entity
    {
        /// <summary>
        /// The name of the ministry.
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        public Guid MinistryId { get; private set; }
        public Ministry Ministry { get; private set; } = null!;


        /// <summary>
        /// Documents optionally linked to this ministry.
        /// </summary>
        // public ICollection<OMSV1.Domain.Entities.Documents.Document> Documents { get; private set; }
        public ICollection<Directorate> Directorates { get; private set; }


        protected GeneralDirectorate()
        {
            Directorates = new List<Directorate>();

            // Documents = new List<OMSV1.Domain.Entities.Documents.Document>();
        }

        public GeneralDirectorate(string name, Guid ministryId) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (ministryId == Guid.Empty)
                throw new ArgumentException("MinistryId must be provided.", nameof(ministryId));

            Name = name;
            MinistryId = ministryId;
        }
      public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            Name = name;
        }
    }
}

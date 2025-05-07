// Ministry.cs
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Ministries
{
    /// <summary>
    /// Represents a government ministry associated with a document.
    /// </summary>
    public class Ministry : Entity
    {
        /// <summary>
        /// The name of the ministry.
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Documents optionally linked to this ministry.
        /// </summary>
        // public ICollection<OMSV1.Domain.Entities.Documents.Document> Documents { get; private set; }
        public ICollection<GeneralDirectorate> GeneralDirectorates { get; private set; }


        protected Ministry()
        {
            GeneralDirectorates = new List<GeneralDirectorate>();
            // Documents = new List<OMSV1.Domain.Entities.Documents.Document>();
        }

        public Ministry(string name) : this()
        {
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

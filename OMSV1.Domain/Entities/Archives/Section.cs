// Ministry.cs
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Sections
{
    /// <summary>
    /// Represents a government ministry associated with a document.
    /// </summary>
    public class Section : Entity
    {
        /// <summary>
        /// The name of the ministry.
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        public Guid DepartmentId { get; private set; }
        public Department Department { get; private set; } = null!;

        public Section(string name, Guid departmentId)
        {
            Name = name;
            DepartmentId = departmentId;
        }


        /// <summary>
        /// Documents optionally linked to this ministry.
        /// </summary>
        // public ICollection<OMSV1.Domain.Entities.Documents.Document> Documents { get; private set; }

        // protected Section()
        // {
        //     // Documents = new List<OMSV1.Domain.Entities.Documents.Document>();
        // }



      public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            Name = name;
        }
    }
}

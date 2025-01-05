using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Lectures
{
    public class LectureType : Entity
    {
        public string Name { get; private set; } // Make setter private to protect it from external modification
        public Guid CompanyId { get; private set; }
        public Company Company { get; private set; }

        public LectureType(string name, Guid companyId)
        {
            Name = name;
            CompanyId = companyId;
        }

        // Public method to update the name of the LectureType
        public void UpdateName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(newName));
            }

            Name = newName; // Update the name property
        }
    }
}

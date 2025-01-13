using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Lectures
{
    public class LectureType(string name, Guid companyId) : Entity
    {
        public string Name { get; private set; } = name;
        public Guid CompanyId { get; private set; } = companyId;
        public Company? Company { get; private set; }

        // Add navigation property for many-to-many relationship
        public ICollection<LectureLectureType> LectureLectureTypes { get; private set; } = new List<LectureLectureType>();

        public void UpdateName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(newName));
            }

            Name = newName;
        }
    }
}
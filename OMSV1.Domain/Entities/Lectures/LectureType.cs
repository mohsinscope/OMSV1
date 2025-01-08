using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Lectures
{
    public class LectureType : Entity
    {
        public string Name { get; private set; }
        public Guid CompanyId { get; private set; }
        public Company Company { get; private set; }
        
        // Add navigation property for many-to-many relationship
        public ICollection<LectureLectureType> LectureLectureTypes { get; private set; }

        public LectureType(string name, Guid companyId)
        {
            Name = name;
            CompanyId = companyId;
            LectureLectureTypes = new List<LectureLectureType>();
        }

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
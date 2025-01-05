using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Companies
{
    public class Company : Entity
    {
        public string Name { get; private set; }
        public ICollection<LectureType> LectureTypes { get; private set; }

        public Company(string name)
        {
            Name = name;
            LectureTypes = new List<LectureType>();
        }

        public void AddLectureType(LectureType lectureType)
        {
            LectureTypes.Add(lectureType);
        }
    }
}

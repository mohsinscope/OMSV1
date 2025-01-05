using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Lectures
{
    public class LectureType : Entity
    {
        public string Name { get; private set; }
        public Guid CompanyId { get; private set; }
        public Company Company { get; private set; }

        public LectureType(string name, Guid companyId)
        {
            Name = name;
            CompanyId = companyId;
        }
    }
}

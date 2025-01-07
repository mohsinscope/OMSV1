using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Lectures
{
    public class Lecture : Entity
    {
        public string Title { get; private set; }
        public DateTime Date { get; private set; }
        public string Note { get; private set; }
        public Guid OfficeId { get; private set; }
        public Guid GovernorateId { get; private set; }
        public Guid ProfileId { get; private set; }
        public Guid? CompanyId { get; private set; }
        public Guid? LectureTypeId { get; private set; }
        
        public Governorate Governorate { get; private set; }
        public Office Office { get; private set; }
        public Profile Profile { get; private set; }
        public Company? Company { get; private set; }
        public LectureType? LectureType { get; private set; }

        public Lecture(string title,
                       DateTime date,
                       string note,
                       Guid officeId,
                       Guid governorateId,
                       Guid profileId,
                       Guid? companyId,
                       Guid? lectureTypeId)
        {
            Title = title;
            Date = date;
            Note = note;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            CompanyId = companyId;
            LectureTypeId = lectureTypeId;
        }

        public void UpdateLectureDetails(string title, DateTime date, string note, Guid officeId, Guid governorateId, Guid profileId, Guid? companyId, Guid? lectureTypeId)
        {
            Title = title;
            Date = date;
            Note = note;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            CompanyId = companyId;
            LectureTypeId = lectureTypeId;
        }
    }
}

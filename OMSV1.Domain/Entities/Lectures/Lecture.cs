// Lecture.cs
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
        // Ensure the DateTime is always UTC
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            private set => _date = DateTime.SpecifyKind(value, DateTimeKind.Utc); // Ensure UTC date
        }
        public string Note { get; private set; }
        public Guid OfficeId { get; private set; }
        public Guid GovernorateId { get; private set; }
        public Guid ProfileId { get; private set; }
        public Guid? CompanyId { get; private set; }
        
        public Governorate Governorate { get; private set; }
        public Office Office { get; private set; }
        public Profile Profile { get; private set; }
        public Company? Company { get; private set; }
        public ICollection<LectureLectureType> LectureLectureTypes { get; private set; }

        public Lecture(string title,
                    DateTime date,
                    string note,
                    Guid officeId,
                    Guid governorateId,
                    Guid profileId,
                    Guid? companyId)
        {
            Title = title;
            Date = DateTime.SpecifyKind(date, DateTimeKind.Utc); // Ensure UTC date
            Note = note;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            CompanyId = companyId;
            LectureLectureTypes = new List<LectureLectureType>();
        }

        public void UpdateLectureDetails(
            string title, 
            DateTime date, 
            string note, 
            Guid officeId, 
            Guid governorateId, 
            Guid profileId, 
            Guid? companyId,
            List<Guid> lectureTypeIds)
        {
            Title = title;
    Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);  // Ensure UTC date
            Note = note;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            CompanyId = companyId;
        }

        public void AddLectureType(Guid lectureTypeId)
        {
            var lectureLectureType = new LectureLectureType(this.Id, lectureTypeId);
            LectureLectureTypes.Add(lectureLectureType);
        }

        public void RemoveLectureType(Guid lectureTypeId)
        {
            var lectureType = LectureLectureTypes.FirstOrDefault(lt => lt.LectureTypeId == lectureTypeId);
            if (lectureType != null)
            {
                LectureLectureTypes.Remove(lectureType);
            }
        }
    }
}
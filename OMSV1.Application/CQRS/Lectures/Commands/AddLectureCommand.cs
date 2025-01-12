using MediatR;
using System;

namespace OMSV1.Application.Commands.Lectures
{
    public class AddLectureCommand : IRequest<Guid> // Returns the ID of the newly created Lecture
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; } = ""; // Default value for Note if not provided

        public Guid OfficeId { get; set; }
        public Guid GovernorateId { get; set; }
        public Guid ProfileId { get; set; }
        public Guid CompanyId { get; set; } // New property for Company
        public List<Guid> LectureTypeIds { get; set; } // Changed to List<Guid>

                public AddLectureCommand(
            string title, 
            DateTime date, 
            string note, 
            Guid officeId, 
            Guid governorateId, 
            Guid profileId, 
            Guid companyId, 
            List<Guid> lectureTypeIds)  // Updated parameter
        {
            Title = title;
            Date = date;
            Note = note;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            CompanyId = companyId;
            LectureTypeIds = lectureTypeIds;
        }
    }
}

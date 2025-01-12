using MediatR;
using System;

namespace OMSV1.Application.Commands.Lectures
{
    public class UpdateLectureCommand : IRequest<bool> // Returns a boolean indicating success
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; }
        public Guid OfficeId { get; set; }
        public Guid GovernorateId { get; set; }
        public Guid ProfileId { get; set; }
        public Guid CompanyId { get; set; } // New property
        public List<Guid> LectureTypeIds { get; set; } // Changed to List<Guid>

        // Constructor to initialize the properties
        public UpdateLectureCommand(Guid id, string title, DateTime date, string? note, Guid officeId, Guid governorateId, Guid profileId, Guid companyId, List<Guid> lectureTypeIds)
        {
            Id = id;
            Title = title;
            Note = note;
            Date = date;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            CompanyId = companyId;
            LectureTypeIds = lectureTypeIds;
        }
    }
}

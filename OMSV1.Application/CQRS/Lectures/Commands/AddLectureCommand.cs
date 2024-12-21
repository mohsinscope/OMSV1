using MediatR;
using System;

namespace OMSV1.Application.Commands.Lectures
{
    public class AddLectureCommand : IRequest<int> // Returns the ID of the newly created Lecture
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; } = ""; // Default value for Note if not provided

        public int OfficeId { get; set; }
        public int GovernorateId { get; set; }
        public int ProfileId { get; set; }

        public AddLectureCommand(string title, DateTime date,string note, int officeId, int governorateId, int profileId)
        {
            Title = title;
            Date = date;
            Note=note;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
        }
    }
}

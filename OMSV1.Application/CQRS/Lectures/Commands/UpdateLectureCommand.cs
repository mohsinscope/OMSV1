using MediatR;
using System;

namespace OMSV1.Application.Commands.Lectures
{
    public class UpdateLectureCommand : IRequest<bool> // Returns a boolean indicating success
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int OfficeId { get; set; }
        public int GovernorateId { get; set; }
        public int ProfileId { get; set; }

        // Constructor to initialize the properties
        public UpdateLectureCommand(int id, string title, DateTime date, int officeId, int governorateId, int profileId)
        {
            Id = id;
            Title = title;
            Date = date;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
        }
    }
}

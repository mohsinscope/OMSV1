using MediatR;
using System;

namespace OMSV1.Application.Queries.Attendances
{
    public class GetUnavailableAttendancesQuery : IRequest<List<string>>
    {
        public DateTime Date { get; set; }
        public int? WorkingHours { get; set; }
        public Guid? GovernorateId { get; set; }

        public GetUnavailableAttendancesQuery(DateTime date, int? workingHours, Guid? governorateId)
        {
            Date = date;
            WorkingHours = workingHours;
            GovernorateId = governorateId;
        }
    }
}

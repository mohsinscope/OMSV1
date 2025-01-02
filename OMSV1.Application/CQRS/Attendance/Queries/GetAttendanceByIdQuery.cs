using MediatR;
using OMSV1.Application.Dtos.Attendances;

namespace OMSV1.Application.Queries.Attendances
{
    public class GetAttendanceByIdQuery : IRequest<AttendanceDto?>
    {
        public Guid Id { get; }

        public GetAttendanceByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

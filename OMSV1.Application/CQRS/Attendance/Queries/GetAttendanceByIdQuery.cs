using MediatR;
using OMSV1.Application.Dtos.Attendances;

namespace OMSV1.Application.Queries.Attendances
{
    public class GetAttendanceByIdQuery : IRequest<AttendanceDto?>
    {
        public int Id { get; }

        public GetAttendanceByIdQuery(int id)
        {
            Id = id;
        }
    }
}

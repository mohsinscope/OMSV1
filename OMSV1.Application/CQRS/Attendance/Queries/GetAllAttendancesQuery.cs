using MediatR;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Attendances
{
    public class GetAllAttendancesQuery : IRequest<PagedList<AttendanceDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllAttendancesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}

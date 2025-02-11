using MediatR;
using OMSV1.Application.Dtos.Dashboard;

namespace OMSV1.Application.Dashboard.Queries
{
    public class GetLastSevenDaysAttendanceQuery : IRequest<DashboardLastSevenDaysAttendanceDto>
    {
    }
}

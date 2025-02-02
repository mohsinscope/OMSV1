using OMSV1.Domain.Entities.Attendances;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSV1.Infrastructure.Interfaces
{
    public interface IAttendanceService
    {
        Task<byte[]> GenerateDailyAttendancePdfAsync(List<Attendance> attendances);
    }
}

using OMSV1.Domain.Entities.Attendances;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSV1.Domain.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<List<Attendance>> GetAttendanceByDateAsync(DateTime date);
    }
}

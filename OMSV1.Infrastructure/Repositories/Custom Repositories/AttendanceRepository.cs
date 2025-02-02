using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSV1.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all attendance records.
        /// </summary>
        /// <returns>List of all attendance records.</returns>
        public async Task<List<Attendance>> GetAllAttendancesAsync()
        {
            return await _context.Attendances
                .Include(a => a.Governorate)  // Include related Governorate
                .Include(a => a.Office)       // Include related Office
                .Include(a => a.Profile)      // Include related Profile
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves attendance records for a specific date.
        /// </summary>
        /// <param name="date">The date for which attendance records are needed.</param>
        /// <returns>List of attendance records for the given date.</returns>
        public async Task<List<Attendance>> GetAttendanceByDateAsync(DateTime date)
        {
            return await _context.Attendances
                .Include(a => a.Governorate)  // Include related Governorate
                .Include(a => a.Office)       // Include related Office
                .Include(a => a.Profile)      // Include related Profile
                .Where(a => a.Date.Date == date.Date) // Filter by the specified date
                .ToListAsync();
        }
    }
}

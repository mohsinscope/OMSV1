using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSV1.Infrastructure.Repositories
{
    public class DamagedPassportRepository : IDamagedPassportRepository
    {
        private readonly AppDbContext _context;

        public DamagedPassportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DamagedPassport>> GetAllDamagedPassportsAsync()
        {
            return await _context.DamagedPassports
                .Include(dp => dp.Governorate) // Include related Governorate
                .Include(dp => dp.Office)      // Include related Office
                .Include(dp => dp.Profile)     // Include related Profile
                .Include(dp => dp.DamagedType) // Include related DamagedType
                .ToListAsync();
        }

        public async Task<List<DamagedPassport>> GetDamagedPassportsByDateAsync(DateTime date)
        {
            return await _context.DamagedPassports
                .Include(dp => dp.Governorate) // Include related Governorate
                .Include(dp => dp.Office)      // Include related Office
                .Include(dp => dp.Profile)     // Include related Profile
                .Include(dp => dp.DamagedType) // Include related DamagedType
                .Where(dp => dp.DateCreated.Date == date.Date)
                .ToListAsync();
        }
    }
}
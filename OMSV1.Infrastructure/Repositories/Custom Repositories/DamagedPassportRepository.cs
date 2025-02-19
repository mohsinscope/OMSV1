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

// public async Task<List<DamagedPassport>> GetDamagedPassportsByDateAsync(DateTime date)
// {
//     // Define the UTC+3 offset.
//     TimeSpan utcPlus3 = TimeSpan.FromHours(3);

//     // Adjust the provided date (assumed to be UTC) so that it represents the local (UTC+3) midnight.
//     DateTime reportDateUtc3 = date.Add(utcPlus3);
//     DateTime localReportDay = reportDateUtc3.Date;

//     // Convert the start and end of the local day back to UTC.
//     DateTime startUtc = localReportDay - utcPlus3;
//     DateTime endUtc = startUtc.AddDays(1);

//     // Log boundaries if desired (or let the controller log them).
//     // _logger.LogInformation("Repository Query: startUtc = {startUtc}, endUtc = {endUtc}", startUtc, endUtc);

//     return await _context.DamagedPassports
//         .Include(dp => dp.Governorate)
//         .Include(dp => dp.Office)
//         .Include(dp => dp.Profile)
//         .Include(dp => dp.DamagedType)
//         .Where(dp => dp.DateCreated >= startUtc && dp.DateCreated < endUtc)
//         .ToListAsync();
// }
public async Task<List<DamagedPassport>> GetDamagedPassportsByDateAsync(DateTime localDate)
{
    // Convert local date (UTC+3) to UTC time range
    var utcPlus3 = TimeSpan.FromHours(3);
    
    // Calculate UTC start and end times
    DateTime startUtc = localDate.Subtract(utcPlus3);  // Convert local midnight to UTC
    DateTime endUtc = startUtc.AddDays(1);            // Add 24 hours for the full day

    return await _context.DamagedPassports
        .Include(dp => dp.Governorate)
        .Include(dp => dp.Office)
        .Include(dp => dp.Profile)
        .Include(dp => dp.DamagedType)
        .Where(dp => dp.DateCreated >= startUtc && dp.DateCreated < endUtc)
        .ToListAsync();
}

        public async Task<List<DamagedPassport>> GetDamagedPassportsByDateCreatedAsync(DateTime date)
        {
            // Calculate the start and end of the given day.
            DateTime startOfDay = date.Date;
            DateTime endOfDay = startOfDay.AddDays(1);

            // Retrieve passports where DateCreated is on the specified day.
            return await _context.DamagedPassports
                .Where(dp => dp.DateCreated >= startOfDay && dp.DateCreated < endOfDay)
                .ToListAsync();
        }
    
    }
}
using MediatR;
using OMSV1.Application.Dashboard.Queries;
using OMSV1.Application.Dtos.Dashboard;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Dashboard.Handlers
{
    public class GetDashboardStatisticsQueryHandler : IRequestHandler<GetDashboardStatisticsQuery, DashboardStatisticsDto>
    {
        private readonly IGenericRepository<Office> _officeRepository;
        private readonly IGenericRepository<Governorate> _governorateRepository;
        private readonly IGenericRepository<DamagedPassport> _damagedPassportRepository;
        private readonly IGenericRepository<Attendance> _attendanceRepository;

        public GetDashboardStatisticsQueryHandler(
            IGenericRepository<Office> officeRepository,
            IGenericRepository<Governorate> governorateRepository,
            IGenericRepository<DamagedPassport> damagedPassportRepository,
            IGenericRepository<Attendance> attendanceRepository)
        {
            _officeRepository = officeRepository;
            _governorateRepository = governorateRepository;
            _damagedPassportRepository = damagedPassportRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<DashboardStatisticsDto> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken)
        {
            // 1. Retrieve all offices and count them.
            var offices = await _officeRepository.GetAllAsync();
            int totalOffices = offices.Count;

            // 2. Retrieve all governorates and count them.
            var governorates = await _governorateRepository.GetAllAsync();
            int totalGovernorates = governorates.Count;

            // 3 & 4. Sum up the staff counts from each office.
            int totalReceivingStaff = offices.Sum(o => o.ReceivingStaff);
            int totalAccountStaff   = offices.Sum(o => o.AccountStaff);
            int totalPrintingStaff  = offices.Sum(o => o.PrintingStaff);
            int totalQualityStaff   = offices.Sum(o => o.QualityStaff);
            int totalDeliveryStaff  = offices.Sum(o => o.DeliveryStaff);

            int totalStaffInAllOffices = totalReceivingStaff + totalAccountStaff +
                                         totalPrintingStaff + totalQualityStaff +
                                         totalDeliveryStaff;

            // 5. Count the number of damaged passports registered today.
            DateTime today = DateTime.UtcNow.Date;
            var damagedPassports = await _damagedPassportRepository.GetAllAsync();
            int totalDamagedPassportsToday = damagedPassports.Count(dp => dp.Date.Date == today);

            // 6. Calculate the attendance percentage for today's attendances,
            // including all offices—even if they didn't record attendance today.
            // a) The expected staff count comes from **all** offices.
            // Multiply by 2 to account for the two shifts.
            int totalExpectedStaff = offices.Sum(o => 
                o.ReceivingStaff + o.AccountStaff + o.PrintingStaff + o.QualityStaff + o.DeliveryStaff) * 2;

            // b) Retrieve today's attendance records.
            var allAttendances = await _attendanceRepository.GetAllAsync();
            var todayAttendances = allAttendances.Where(a => a.Date.Date == today).ToList();

            // Build a lookup for attendance sums by OfficeId.
            // In case an office has multiple attendance records today, we sum them.
            var attendanceByOffice = todayAttendances
                .GroupBy(a => a.OfficeId)
                .ToDictionary(
                    g => g.Key, 
                    g => g.Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff)
                );

            // c) Sum up attended staff for every office.
            int totalAttendedStaff = offices.Sum(o =>
                attendanceByOffice.TryGetValue(o.Id, out var attendedCount) ? attendedCount : 0);

            // Round the attendance percentage to 2 decimal places.
            decimal attendancePercentage = totalExpectedStaff > 0 
                ? Math.Round(((decimal)totalAttendedStaff * 100) / totalExpectedStaff, 2)
                : 0;

            return new DashboardStatisticsDto
            {
                TotalOffices = totalOffices,
                TotalGovernorates = totalGovernorates,
                TotalReceivingStaff = totalReceivingStaff,
                TotalAccountStaff = totalAccountStaff,
                TotalPrintingStaff = totalPrintingStaff,
                TotalQualityStaff = totalQualityStaff,
                TotalDeliveryStaff = totalDeliveryStaff,
                TotalStaffInAllOffices = totalStaffInAllOffices,
                TotalDamagedPassportsToday = totalDamagedPassportsToday,
                AttendancePercentage = attendancePercentage
            };
        }
    }
}

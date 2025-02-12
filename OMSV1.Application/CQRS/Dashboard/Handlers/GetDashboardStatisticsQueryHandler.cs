using MediatR;
using OMSV1.Application.Dashboard.Queries;
using OMSV1.Application.Dtos.Dashboard;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Enums; // Needed for WorkingHours enum
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

            // 5. Count the number of damaged passports registered this month.
            DateTime today = DateTime.UtcNow.Date;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            var damagedPassports = await _damagedPassportRepository.GetAllAsync();
            int totalDamagedPassportsThisMonth = damagedPassports
                .Count(dp => dp.DateCreated.Date >= startOfMonth && dp.DateCreated.Date <= today);

            // 6. Calculate the attendance percentages separately for the two shifts.
            // a) The expected staff count for one shift (each shift uses the same staff count from all offices).
            int totalExpectedStaffPerShift = offices.Sum(o =>
                o.ReceivingStaff + o.AccountStaff + o.PrintingStaff + o.QualityStaff + o.DeliveryStaff);

            // b) Retrieve today's attendance records.
            var allAttendances = await _attendanceRepository.GetAllAsync();
            var todayAttendances = allAttendances.Where(a => a.Date.Date == today).ToList();

            // c) Filter today's attendances by shift.
            var morningAttendances = todayAttendances
                .Where(a => a.WorkingHours.HasFlag(WorkingHours.Morning))
                .ToList();

            var eveningAttendances = todayAttendances
                .Where(a => a.WorkingHours.HasFlag(WorkingHours.Evening))
                .ToList();

            // d) Group attendances by OfficeId and sum the attended staff for each shift.
            var attendanceByOfficeMorning = morningAttendances
                .GroupBy(a => a.OfficeId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff)
                );

            var attendanceByOfficeEvening = eveningAttendances
                .GroupBy(a => a.OfficeId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff)
                );

            // e) Sum up attended staff across all offices for morning and evening shifts.
            int totalAttendedStaffMorning = offices.Sum(o =>
                attendanceByOfficeMorning.TryGetValue(o.Id, out var attendedCount) ? attendedCount : 0);

            int totalAttendedStaffEvening = offices.Sum(o =>
                attendanceByOfficeEvening.TryGetValue(o.Id, out var attendedCount) ? attendedCount : 0);

            // f) Calculate the attendance percentages for each shift.
            decimal attendancePercentageMorning = totalExpectedStaffPerShift > 0
                ? Math.Round(((decimal)totalAttendedStaffMorning * 100) / totalExpectedStaffPerShift, 2)
                : 0;

            decimal attendancePercentageEvening = totalExpectedStaffPerShift > 0
                ? Math.Round(((decimal)totalAttendedStaffEvening * 100) / totalExpectedStaffPerShift, 2)
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
                // Note: if you want to rename this property to "TotalDamagedPassportsThisMonth" in your DTO, adjust accordingly.
                TotalDamagedPassportsToday = totalDamagedPassportsThisMonth,
                AttendancePercentageMorning = attendancePercentageMorning,
                AttendancePercentageEvening = attendancePercentageEvening
            };
        }
    }
}

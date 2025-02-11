using MediatR;
using OMSV1.Application.Dashboard.Queries;
using OMSV1.Application.Dtos.Dashboard;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Dashboard.Handlers
{
    public class GetLastSevenDaysAttendanceQueryHandler : IRequestHandler<GetLastSevenDaysAttendanceQuery, DashboardLastSevenDaysAttendanceDto>
    {
        private readonly IGenericRepository<Office> _officeRepository;
        private readonly IGenericRepository<Attendance> _attendanceRepository;

        public GetLastSevenDaysAttendanceQueryHandler(
            IGenericRepository<Office> officeRepository,
            IGenericRepository<Attendance> attendanceRepository)
        {
            _officeRepository = officeRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<DashboardLastSevenDaysAttendanceDto> Handle(GetLastSevenDaysAttendanceQuery request, CancellationToken cancellationToken)
        {
            // 1. Retrieve all offices.
            var offices = await _officeRepository.GetAllAsync();

            // Adjust expected staff count for two shifts by multiplying by 2.
            int totalExpectedStaff = offices.Sum(o => 
                o.ReceivingStaff + o.AccountStaff + o.PrintingStaff + o.QualityStaff + o.DeliveryStaff) * 2;

            // 2. Retrieve all attendance records.
            var allAttendances = await _attendanceRepository.GetAllAsync();

            // 3. Prepare the list to hold attendance percentages for each day.
            var dailyAttendanceList = new List<DailyAttendanceDto>();

            // 4. Determine today's date (using UTC for consistency).
            DateTime today = DateTime.UtcNow.Date;

            // 5. For each of the last 7 days (today and the 6 previous days)
            for (int i = 0; i < 7; i++)
            {
                DateTime targetDate = today.AddDays(-i);

                // a) Filter attendance records for the target date.
                var attendancesForDate = allAttendances
                    .Where(a => a.Date.Date == targetDate)
                    .ToList();

                // b) Group by OfficeId and sum the attended staff numbers.
                var attendanceByOffice = attendancesForDate
                    .GroupBy(a => a.OfficeId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff)
                    );

                // c) Sum the attended staff for all offices.
                int totalAttendedStaff = offices.Sum(o =>
                    attendanceByOffice.TryGetValue(o.Id, out var attendedCount) ? attendedCount : 0);

                // d) Calculate the attendance percentage for the day.
                decimal attendancePercentage = totalExpectedStaff > 0
                    ? Math.Round(((decimal)totalAttendedStaff * 100) / totalExpectedStaff, 2)
                    : 0;

                // e) Add the daily result to the list.
                dailyAttendanceList.Add(new DailyAttendanceDto
                {
                    Date = targetDate,
                    AttendancePercentage = attendancePercentage
                });
            }

            // Optional: Reverse the list so that it is in chronological order (oldest to newest)
            dailyAttendanceList.Reverse();

            // 6. Return the DTO containing the last 7 days' attendance percentages.
            return new DashboardLastSevenDaysAttendanceDto
            {
                DailyAttendance = dailyAttendanceList
            };
        }
    }
}

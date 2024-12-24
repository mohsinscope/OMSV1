using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Attendances;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Attendances
{
    public class SearchAttendanceStatisticsHandler : IRequestHandler<SearchAttendanceStatisticsQuery, AttendanceStatisticsDto>
    {
        private readonly IGenericRepository<Attendance> _attendanceRepo;
        private readonly IGenericRepository<Office> _officeRepo;

        public SearchAttendanceStatisticsHandler(
            IGenericRepository<Attendance> attendanceRepo,
            IGenericRepository<Office> officeRepo)
        {
            _attendanceRepo = attendanceRepo;
            _officeRepo = officeRepo;
        }

            public async Task<AttendanceStatisticsDto> Handle(SearchAttendanceStatisticsQuery request, CancellationToken cancellationToken)
            {
                // Fetch governorate-specific attendance data
                var attendanceQuery = _attendanceRepo.ListAsQueryable(new FilterAttendanceStatisticsSpecification(
                    officeId: request.OfficeId,
                    governorateId: request.GovernorateId,
                    workingHours: request.WorkingHours,
                    date: request.Date
                ));

                var totalAttendance = await attendanceQuery
                    .GroupBy(a => a.OfficeId)
                    .Select(group => new
                    {
                        OfficeId = group.Key,
                        TotalReceivingStaff = group.Sum(a => a.ReceivingStaff),
                        TotalAccountStaff = group.Sum(a => a.AccountStaff),
                        TotalPrintingStaff = group.Sum(a => a.PrintingStaff),
                        TotalQualityStaff = group.Sum(a => a.QualityStaff),
                        TotalDeliveryStaff = group.Sum(a => a.DeliveryStaff)
                    }).ToListAsync(cancellationToken);

                // Fetch office data filtered by Governorate ID
                var offices = await _officeRepo.GetAllAsQueryable()
                    .Where(o => !request.GovernorateId.HasValue || o.GovernorateId == request.GovernorateId)
                    .Select(o => new OfficeStatisticsDto
                    {
                        Id = o.Id,
                        Name = o.Name,
                        ReceivingStaff = o.ReceivingStaff,
                        AccountStaff = o.AccountStaff,
                        PrintingStaff = o.PrintingStaff,
                        QualityStaff = o.QualityStaff,
                        DeliveryStaff = o.DeliveryStaff
                    })
                    .ToListAsync(cancellationToken);

                // Total staff counts
                var totalStaffCount = offices.Sum(o => o.ReceivingStaff + o.AccountStaff + o.PrintingStaff + o.QualityStaff + o.DeliveryStaff);
                var officeStaffCount = request.OfficeId.HasValue
                    ? offices.Where(o => o.Id == request.OfficeId).Sum(o => o.ReceivingStaff + o.AccountStaff + o.PrintingStaff + o.QualityStaff + o.DeliveryStaff)
                    : 0;

                // Calculate available staff based on attendance
                var availableStaff = totalAttendance.Sum(a => a.TotalReceivingStaff + a.TotalAccountStaff + a.TotalPrintingStaff + a.TotalQualityStaff + a.TotalDeliveryStaff);
                var availableStaffInOffice = request.OfficeId.HasValue
                    ? totalAttendance.Where(a => a.OfficeId == request.OfficeId).Sum(a => a.TotalReceivingStaff + a.TotalAccountStaff + a.TotalPrintingStaff + a.TotalQualityStaff + a.TotalDeliveryStaff)
                    : 0;

                // Calculate total specific staff if a staff type is provided
                int GetAvailableSpecificStaff(Func<dynamic, int> selector) =>
                    request.StaffType != null ? totalAttendance.Sum(selector) : 0;

                var availableSpecificStaff = GetAvailableSpecificStaff(a => request.StaffType switch
                {
                    "ReceivingStaff" => a.TotalReceivingStaff,
                    "AccountStaff" => a.TotalAccountStaff,
                    "PrintingStaff" => a.TotalPrintingStaff,
                    "QualityStaff" => a.TotalQualityStaff,
                    "DeliveryStaff" => a.TotalDeliveryStaff,
                    _ => 0
                });

                var totalSpecificStaff = offices.Sum(o => request.StaffType switch
                {
                    "ReceivingStaff" => o.ReceivingStaff,
                    "AccountStaff" => o.AccountStaff,
                    "PrintingStaff" => o.PrintingStaff,
                    "QualityStaff" => o.QualityStaff,
                    "DeliveryStaff" => o.DeliveryStaff,
                    _ => 0
                });

                var totalSpecificStaffInOffice = offices.Where(o => o.Id == request.OfficeId).Sum(o => request.StaffType switch
                {
                    "ReceivingStaff" => o.ReceivingStaff,
                    "AccountStaff" => o.AccountStaff,
                    "PrintingStaff" => o.PrintingStaff,
                    "QualityStaff" => o.QualityStaff,
                    "DeliveryStaff" => o.DeliveryStaff,
                    _ => 0
                });

                // Calculate percentages
                double CalculatePercentage(int available, int total) => total > 0 ? Math.Round((double)available / total * 100, 2) : 0;

                return new AttendanceStatisticsDto
                {
                    TotalStaffCount = totalStaffCount,
                    TotalStaffInOffice = officeStaffCount,
                    AvailableStaff = availableStaff,
                    AvailableStaffInOffice = availableStaffInOffice,
                    AvailableSpecificStaff = availableSpecificStaff,
                    TotalSpecificStaff = totalSpecificStaff,
                    TotalSpecificStaffInOffice = totalSpecificStaffInOffice, // Ensure this is properly assigned
                    AvailableStaffPercentage = CalculatePercentage(availableStaff, totalStaffCount),
                    AvailableStaffInOfficePercentage = CalculatePercentage(availableStaffInOffice, officeStaffCount),
                    AvailableSpecificStaffPercentage = CalculatePercentage(availableSpecificStaff, totalSpecificStaff)
                };
            }

    }
}

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
            // Fetch attendance data with only required fields
            var attendances = await _attendanceRepo.ListAsQueryable(new FilterAttendanceStatisticsSpecification(
                officeId: request.OfficeId,
                governorateId: request.GovernorateId,
                workingHours: request.WorkingHours,
                date: request.Date
            ))
            .Select(a => new
            {
                a.OfficeId,
                a.ReceivingStaff,
                a.AccountStaff,
                a.PrintingStaff,
                a.QualityStaff,
                a.DeliveryStaff
            }).ToListAsync(cancellationToken);

            // Fetch office data, filtering by Governorate ID if provided
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
            }).ToListAsync(cancellationToken);

            // Total staff counts
            var totalStaffCount = offices.Sum(o => o.ReceivingStaff + o.AccountStaff + o.PrintingStaff + o.QualityStaff + o.DeliveryStaff);
            var officeStaffCount = request.OfficeId.HasValue
                ? offices.Where(o => o.Id == request.OfficeId).Sum(o => o.ReceivingStaff + o.AccountStaff + o.PrintingStaff + o.QualityStaff + o.DeliveryStaff)
                : 0;

            // Calculate available staff based on attendance
            var availableStaff = attendances.Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff);
            var availableStaffInOffice = request.OfficeId.HasValue
                ? attendances.Where(a => a.OfficeId == request.OfficeId)
                    .Sum(a => a.ReceivingStaff + a.AccountStaff + a.PrintingStaff + a.QualityStaff + a.DeliveryStaff)
                : 0;

            // Calculate total specific staff
            int CalculateTotalSpecificStaff(Func<OfficeStatisticsDto, int> selector) =>
                offices.Sum(selector);

            int CalculateTotalSpecificStaffInOffice(Func<OfficeStatisticsDto, int> selector) =>
                request.OfficeId.HasValue
                    ? offices.Where(o => o.Id == request.OfficeId).Sum(selector)
                    : 0;

            // Optional: Filter by specific staff type
            int GetAvailableSpecificStaff(Func<dynamic, int> selector) =>
                request.StaffType != null ? attendances.Sum(selector) : 0;

            var availableSpecificStaff = GetAvailableSpecificStaff(a => request.StaffType switch
            {
                "ReceivingStaff" => a.ReceivingStaff,
                "AccountStaff" => a.AccountStaff,
                "PrintingStaff" => a.PrintingStaff,
                "QualityStaff" => a.QualityStaff,
                "DeliveryStaff" => a.DeliveryStaff,
                _ => 0
            });

            var totalSpecificStaff = CalculateTotalSpecificStaff(o => request.StaffType switch
            {
                "ReceivingStaff" => o.ReceivingStaff,
                "AccountStaff" => o.AccountStaff,
                "PrintingStaff" => o.PrintingStaff,
                "QualityStaff" => o.QualityStaff,
                "DeliveryStaff" => o.DeliveryStaff,
                _ => 0
            });

            var totalSpecificStaffInOffice = CalculateTotalSpecificStaffInOffice(o => request.StaffType switch
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

                // New fields
                TotalSpecificStaff = totalSpecificStaff,
                TotalSpecificStaffInOffice = totalSpecificStaffInOffice,

                // Percentages
                AvailableStaffPercentage = CalculatePercentage(availableStaff, totalStaffCount),
                AvailableStaffInOfficePercentage = CalculatePercentage(availableStaffInOffice, officeStaffCount),
                AvailableSpecificStaffPercentage = CalculatePercentage(availableSpecificStaff, totalSpecificStaff)
            };
        }



    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Attendances;

namespace OMSV1.Application.Queries.Attendances
{
    public class SearchAttendanceTypeStatisticsHandler : IRequestHandler<SearchAttendanceTypeStatisticsQuery, List<AttendanceTypeStatisticsDto>>
    {
        private readonly IGenericRepository<Attendance> _attendanceRepo;
        private readonly IGenericRepository<Office> _officeRepo;

        public SearchAttendanceTypeStatisticsHandler(
            IGenericRepository<Attendance> attendanceRepo,
            IGenericRepository<Office> officeRepo)
        {
            _attendanceRepo = attendanceRepo;
            _officeRepo = officeRepo;
        }

        public async Task<List<AttendanceTypeStatisticsDto>> Handle(SearchAttendanceTypeStatisticsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.StaffType) || !request.Date.HasValue)
            {
                throw new ArgumentException("StaffType and Date are required.");
            }

            // Fetch attendance data based on filters using your specification.
            var attendanceQuery = _attendanceRepo.ListAsQueryable(new FilterAttendanceStatisticsSpecification(
                officeId: request.OfficeId,
                governorateId: request.GovernorateId,
                workingHours: request.WorkingHours,
                date: request.Date
            ));

            // Group attendance data by OfficeId and calculate available staff for the specified staff type.
            var attendanceStatistics = await attendanceQuery
                .GroupBy(a => a.OfficeId)
                .Select(group => new
                {
                    OfficeId = group.Key,
                    AvailableStaff = group.Sum(a =>
                        request.StaffType == "ReceivingStaff" ? a.ReceivingStaff :
                        request.StaffType == "AccountStaff" ? a.AccountStaff :
                        request.StaffType == "PrintingStaff" ? a.PrintingStaff :
                        request.StaffType == "QualityStaff" ? a.QualityStaff :
                        request.StaffType == "DeliveryStaff" ? a.DeliveryStaff :
                        0)
                })
                .ToListAsync(cancellationToken);

            // Fetch office data filtered by Governorate ID and Office ID.
            // IMPORTANT: Include the office Code and order by it.
            var officesQuery = _officeRepo.GetAllAsQueryable()
                .Where(o => !request.GovernorateId.HasValue || o.GovernorateId == request.GovernorateId)
                .Where(o => !request.OfficeId.HasValue || o.Id == request.OfficeId);

            var offices = await officesQuery
                .Select(o => new
                {
                    o.Id,
                    o.Name,
                    o.Code, // Retrieve the Code from the Office entity.
                    TotalStaff = request.StaffType == "ReceivingStaff" ? o.ReceivingStaff :
                                 request.StaffType == "AccountStaff" ? o.AccountStaff :
                                 request.StaffType == "PrintingStaff" ? o.PrintingStaff :
                                 request.StaffType == "QualityStaff" ? o.QualityStaff :
                                 request.StaffType == "DeliveryStaff" ? o.DeliveryStaff :
                                 0
                })
                .OrderBy(o => o.Code)  // Order offices by Code.
                .ToListAsync(cancellationToken);

            // Combine attendance statistics with office data.
            // If you want the final DTOs to be sorted by office code, you can order the result here.
            var result = offices
                .GroupJoin(attendanceStatistics,
                    office => office.Id,
                    stat => stat.OfficeId,
                    (office, stats) => new AttendanceTypeStatisticsDto
                    {
                        OfficeName = office.Name,
                        // Optionally, include OfficeCode in your DTO if you wish to display it:
                        AvailableStaff = stats.Sum(s => s.AvailableStaff),
                        TotalStaff = office.TotalStaff,
                        StaffType = request.StaffType
                    })
                .ToList();

            return result;
        }
    }
}

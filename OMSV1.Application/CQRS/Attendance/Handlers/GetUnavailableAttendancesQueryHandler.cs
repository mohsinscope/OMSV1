using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Queries.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Attendances;

namespace OMSV1.Application.Handlers.Attendances
{
    public class GetUnavailableAttendancesQueryHandler : IRequestHandler<GetUnavailableAttendancesQuery, List<string>>
    {
        private readonly IGenericRepository<Attendance> _attendanceRepository;
        private readonly IGenericRepository<Office> _officeRepository;

        public GetUnavailableAttendancesQueryHandler(
            IGenericRepository<Attendance> attendanceRepository,
            IGenericRepository<Office> officeRepository)
        {
            _attendanceRepository = attendanceRepository;
            _officeRepository = officeRepository;
        }

        public async Task<List<string>> Handle(GetUnavailableAttendancesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Get offices filtered by governorate if specified
                var officesQuery = _officeRepository.GetAllAsQueryable();
                if (request.GovernorateId.HasValue)
                {
                    officesQuery = officesQuery.Where(o => o.GovernorateId == request.GovernorateId);
                }
                var offices = await officesQuery.ToListAsync(cancellationToken);


                // Step 2: Create the attendance specification
                var attendanceSpec = new FilterAttendanceStatisticsSpecification(
                    date: request.Date,
                    workingHours: request.WorkingHours,
                    governorateId: request.GovernorateId);

                // Step 3: Get attendance counts for the specified criteria
                var attendanceData = await _attendanceRepository.GetAllAsQueryable()
                    .Where(attendanceSpec.Criteria)
                    .GroupBy(a => a.OfficeId)
                    .Select(group => new
                    {
                        OfficeId = group.Key,
                        TotalStaff = group.Sum(a => 
                            a.ReceivingStaff + 
                            a.AccountStaff + 
                            a.PrintingStaff + 
                            a.QualityStaff + 
                            a.DeliveryStaff)
                    })
                    .ToListAsync(cancellationToken);
                // Step 4: Find offices with zero attendance count or no attendance records
                var unavailableOffices = offices
                    .Where(office => 
                        !attendanceData.Any(ad => ad.OfficeId == office.Id) || // No attendance record
                        attendanceData.Any(ad => ad.OfficeId == office.Id && ad.TotalStaff == 0) // Or total staff is 0
                    )
                    .Select(o => o.Name)
                    .ToList();
                return unavailableOffices;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving unavailable attendances.", ex);
            }
        }
    }
}
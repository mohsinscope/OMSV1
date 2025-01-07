using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.CQRS.Attendance.Queries;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Specifications.Attendances;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.CQRS.Attendance.Handlers
{
    public class GetAttendanceStatisticsInOfficeQueryHandler : IRequestHandler<GetAttendanceStatisticsInOfficeQuery, AttendanceStatisticsInOfficeDto>
    {
        private readonly IGenericRepository<OMSV1.Domain.Entities.Attendances.Attendance> _attendanceRepository;
        private readonly IGenericRepository<Office> _officeRepository;

        public GetAttendanceStatisticsInOfficeQueryHandler(
            IGenericRepository<OMSV1.Domain.Entities.Attendances.Attendance> attendanceRepository,
            IGenericRepository<Office> officeRepository)
        {
            _attendanceRepository = attendanceRepository;
            _officeRepository = officeRepository;
        }

        public async Task<AttendanceStatisticsInOfficeDto> Handle(GetAttendanceStatisticsInOfficeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Define the attendance filtering specification
            var specification = new FilterAttendanceInOfficesStatisticsSpecification(
                workingHours: request.WorkingHours,
                date: request.Date,
                officeId: request.OfficeId
            );

            // Retrieve attendance data using ListAsQueryable
            var attendancesQuery = _attendanceRepository.ListAsQueryable(specification);

            // Retrieve office data
            var office = await _officeRepository.GetByIdAsync(request.OfficeId.Value);
            if (office == null)
                throw new Exception($"Office with ID {request.OfficeId} not found");

            // Calculate total staff in the office
            var totalStaff = office.ReceivingStaff + office.AccountStaff + office.PrintingStaff +
                            office.QualityStaff + office.DeliveryStaff;

            // Aggregate attendance data to calculate available staff
            var attendanceData = await attendancesQuery
                .GroupBy(a => 1) // Group by a constant to aggregate all records
                .Select(g => new
                {
                    AvailableReceivingStaff = g.Sum(a => a.ReceivingStaff),
                    AvailableAccountStaff = g.Sum(a => a.AccountStaff),
                    AvailablePrintingStaff = g.Sum(a => a.PrintingStaff),
                    AvailableQualityStaff = g.Sum(a => a.QualityStaff),
                    AvailableDeliveryStaff = g.Sum(a => a.DeliveryStaff),
                    // Total available staff is not needed anymore for AvailableStaffInOffice
                })
                .FirstOrDefaultAsync(cancellationToken);

            // Assign attendance data
            var availableReceivingStaff = attendanceData?.AvailableReceivingStaff ?? 0;
            var availableAccountStaff = attendanceData?.AvailableAccountStaff ?? 0;
            var availablePrintingStaff = attendanceData?.AvailablePrintingStaff ?? 0;
            var availableQualityStaff = attendanceData?.AvailableQualityStaff ?? 0;
            var availableDeliveryStaff = attendanceData?.AvailableDeliveryStaff ?? 0;

            // Calculate the total available staff in the office (sum of available staff in all categories)
            var availableStaffInOffice = availableReceivingStaff + availableAccountStaff + availablePrintingStaff +
                                        availableQualityStaff + availableDeliveryStaff;

            // Construct and return the DTO
            return new AttendanceStatisticsInOfficeDto
            {
                TotalStaffInOffice = totalStaff,
                AvailableStaffInOffice = availableStaffInOffice, // Updated calculation
                ReceivingStaffTotal = office.ReceivingStaff,
                ReceivingStaffAvailable = availableReceivingStaff,
                AccountStaffTotal = office.AccountStaff,
                AccountStaffAvailable = availableAccountStaff,
                PrintingStaffTotal = office.PrintingStaff,
                PrintingStaffAvailable = availablePrintingStaff,
                QualityStaffTotal = office.QualityStaff,
                QualityStaffAvailable = availableQualityStaff,
                DeliveryStaffTotal = office.DeliveryStaff,
                DeliveryStaffAvailable = availableDeliveryStaff
            };
        }
        catch (Exception ex)
        {
            // Log and rethrow the exception
            throw new Exception("An error occurred while retrieving attendance statistics.", ex);
        }
    }

    }
}
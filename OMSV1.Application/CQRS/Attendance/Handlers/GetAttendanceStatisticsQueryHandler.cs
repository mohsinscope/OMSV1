// using System;
// using AutoMapper;
// using MediatR;
// using OMSV1.Application.CQRS.Attendance.Commands;
// using OMSV1.Application.Dtos.Attendance;
// using OMSV1.Domain.Entities.Offices;
// using OMSV1.Domain.SeedWork;
// using OMSV1.Domain.Specifications.Attendances;
// using OMSV1.Domain.Specifications.Offices;
// namespace OMSV1.Application.CQRS.Attendance.Handlers;

// public class GetAttendanceStatisticsQueryHandler : IRequestHandler<GetAttendanceStatisticsQuery, AttendanceStatisticsDto>
// {
//     private readonly IGenericRepository<Office> _officeRepository;
//     private readonly IGenericRepository<OMSV1.Domain.Entities.Attendances.Attendance> _attendanceRepository;

//     private readonly IMapper _mapper;

//     public GetAttendanceStatisticsQueryHandler(IGenericRepository<Office> officeRpository,IGenericRepository<OMSV1.Domain.Entities.Attendances.Attendance> attendanceRepository, IMapper mapper)
//     {
//         _officeRepository = officeRpository;
//         _attendanceRepository = attendanceRepository;

//         _mapper = mapper;
//     }

//     public async Task<AttendanceStatisticsDto> Handle(GetAttendanceStatisticsQuery request, CancellationToken cancellationToken)
//     {
//         // Get filtered offices
//         var officeSpec = new FilterOfficeSpecification(request.GovernorateId, request.OfficeId);
//         var offices = await _officeRepository.ListAsync(officeSpec, cancellationToken);

//         // Get filtered attendance records
//         var attendanceSpec = new FilterAttendanceSpecification(
//             request.GovernorateId,
//             request.OfficeId,
//             request.FromDate,
//             request.ToDate
//         );
//         var attendances = await _attendanceRepository.ListAsync(attendanceSpec, cancellationToken);

//         var result = new AttendanceStatisticsDto
//         {
//             TotalOffices = offices.Count,
//             TotalStaff = CalculateTotalStaff(offices),
//             TotalAttendance = CalculateTotalAttendance(attendances),
//             OfficesAttendance = CalculateOfficeAttendance(offices, attendances)
//         };

//         result.OverallAttendancePercentage = CalculatePercentage(
//             result.TotalAttendance.Total,
//             result.TotalStaff.Total
//         );

//         return result;
//     }
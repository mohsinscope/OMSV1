using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Queries.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;
using System.Linq;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Handlers.Attendances
{
    public class GetAttendanceByIdQueryHandler : IRequestHandler<GetAttendanceByIdQuery, AttendanceDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAttendanceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AttendanceDto?> Handle(GetAttendanceByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Attendance>().GetAllAsQueryable()
                    .Where(a => a.Id == request.Id)
                    .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider);

                // Fetch the attendance record
                var attendance = await query.FirstOrDefaultAsync(cancellationToken);

                if (attendance == null)
                {
                    // If no attendance record is found, throw an exception
                    throw new HandlerException($"Attendance with ID {request.Id} not found.");
                }

                return attendance;
            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions
                throw new HandlerException("An error occurred while fetching the attendance record.", ex);
            }
        }
    }
}

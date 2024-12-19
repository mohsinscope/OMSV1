using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Queries.Attendances;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.SeedWork;
using System.Linq;

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
            var query = _unitOfWork.Repository<Attendance>().GetAllAsQueryable()
                .Where(a => a.Id == request.Id)
                .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}

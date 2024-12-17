using MediatR;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Entities.DamagedDevices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OMSV1.Application.Queries.LOV
{
    public class GetDamagedDeviceTypeQueryHandler : IRequestHandler<GetDamagedDeviceTypeQuery, DamagedDeviceTypeDto>
    {
        private readonly AppDbContext _context;

        public GetDamagedDeviceTypeQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DamagedDeviceTypeDto> Handle(GetDamagedDeviceTypeQuery request, CancellationToken cancellationToken)
        {
            var damagedDeviceType = await _context.DamagedDeviceTypes
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (damagedDeviceType == null)
                return null;

            return new DamagedDeviceTypeDto
            {
                Id = damagedDeviceType.Id,
                Name = damagedDeviceType.Name,
                Description = damagedDeviceType.Description
            };
        }
    }
}

using MediatR;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Domain.Entities.DamagedDevices;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Commands.LOV
{
    public class UpdateDamagedDeviceTypeCommandHandler : IRequestHandler<UpdateDamagedDeviceTypeCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateDamagedDeviceTypeCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateDamagedDeviceTypeCommand request, CancellationToken cancellationToken)
        {
            var damagedDeviceType = await _context.DamagedDeviceTypes
                .FindAsync(request.Id);

            if (damagedDeviceType == null)
                return false;

            damagedDeviceType.Update(request.Name, request.Description);

            _context.DamagedDeviceTypes.Update(damagedDeviceType);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

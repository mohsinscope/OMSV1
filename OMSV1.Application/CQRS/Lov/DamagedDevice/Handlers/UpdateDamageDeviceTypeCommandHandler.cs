using MediatR;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Domain.Entities.DamagedDevices;
using System;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

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
            try
            {
                // Retrieve the damaged device type by ID
                var damagedDeviceType = await _context.DamagedDeviceTypes
                    .FindAsync(request.Id);

                // If not found, return false
                if (damagedDeviceType == null)
                    return false;

                // Update the entity
                damagedDeviceType.Update(request.Name, request.Description);

                // Update the entity in the context and save the changes
                _context.DamagedDeviceTypes.Update(damagedDeviceType);
                await _context.SaveChangesAsync(cancellationToken);

                return true; // Return true if the update was successful
            }
            catch (Exception ex)
            {
                // Throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while updating the damaged device type.", ex);
            }
        }
    }
}

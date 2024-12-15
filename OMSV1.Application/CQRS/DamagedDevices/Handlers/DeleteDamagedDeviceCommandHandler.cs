using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class DeleteDamagedDeviceCommandHandler : IRequestHandler<DeleteDamagedDeviceCommand, bool>
    {
        private readonly IGenericRepository<DamagedDevice> _repository;

        public DeleteDamagedDeviceCommandHandler(IGenericRepository<DamagedDevice> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteDamagedDeviceCommand request, CancellationToken cancellationToken)
        {
            var damagedDevice = await _repository.GetByIdAsync(request.Id);

            if (damagedDevice == null) return false; // Device not found

            await _repository.DeleteAsync(damagedDevice);
            return true;
        }
    }
}

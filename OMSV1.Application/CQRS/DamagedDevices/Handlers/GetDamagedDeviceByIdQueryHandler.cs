using MediatR;
using OMSV1.Application.Queries.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class GetDamagedDeviceByIdQueryHandler : IRequestHandler<GetDamagedDeviceByIdQuery, DamagedDevice?>
    {
        private readonly IGenericRepository<DamagedDevice> _repository;

        public GetDamagedDeviceByIdQueryHandler(IGenericRepository<DamagedDevice> repository)
        {
            _repository = repository;
        }

        public async Task<DamagedDevice?> Handle(GetDamagedDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdWithIncludesAsync(
                request.Id,
                dd => dd.DamagedDeviceTypes!,
                dd => dd.DeviceType!,
                dd => dd.Governorate!,
                dd => dd.Office!
            );
        }
    }
}

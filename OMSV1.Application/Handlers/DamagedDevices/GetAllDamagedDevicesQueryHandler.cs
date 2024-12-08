using MediatR;
using OMSV1.Application.Queries.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class GetAllDamagedDevicesQueryHandler(IGenericRepository<DamagedDevice> repository) : IRequestHandler<GetAllDamagedDevicesQuery, IReadOnlyList<DamagedDevice>>
    {
        private readonly IGenericRepository<DamagedDevice> _repository = repository;

        public async Task<IReadOnlyList<DamagedDevice>> Handle(GetAllDamagedDevicesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}

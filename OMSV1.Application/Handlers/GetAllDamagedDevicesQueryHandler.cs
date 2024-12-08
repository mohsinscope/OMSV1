using MediatR;
using OMSV1.Application.Queries;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers
{
    public class GetAllDamagedDevicesQueryHandler : IRequestHandler<GetAllDamagedDevicesQuery, IReadOnlyList<DamagedDevice>>
    {
        private readonly IGenericRepository<DamagedDevice> _repository;

        public GetAllDamagedDevicesQueryHandler(IGenericRepository<DamagedDevice> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<DamagedDevice>> Handle(GetAllDamagedDevicesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}

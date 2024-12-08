using MediatR;
using OMSV1.Application.Queries;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers
{
    public class GetAllOfficesQueryHandler : IRequestHandler<GetAllOfficesQuery, IReadOnlyList<Office>>
    {
        private readonly IGenericRepository<Office> _repository;

        public GetAllOfficesQueryHandler(IGenericRepository<Office> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<Office>> Handle(GetAllOfficesQuery request, CancellationToken cancellationToken)
        {
            // Fetch all Office entities from the repository
            var offices = await _repository.GetAllAsync();
            return offices;
        }
    }
}

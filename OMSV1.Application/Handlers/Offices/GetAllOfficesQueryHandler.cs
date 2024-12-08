using MediatR;
using OMSV1.Application.Queries.Offices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Offices
{
    public class GetAllOfficesQueryHandler(IGenericRepository<Office> repository) : IRequestHandler<GetAllOfficesQuery, IReadOnlyList<Office>>
    {
        private readonly IGenericRepository<Office> _repository = repository;

        public async Task<IReadOnlyList<Office>> Handle(GetAllOfficesQuery request, CancellationToken cancellationToken)
        {
            // Fetch all Office entities from the repository
            var offices = await _repository.GetAllAsync();
            return offices;
        }
    }
}

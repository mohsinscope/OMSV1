using MediatR;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Governorates
{
    public class GetAllGovernoratesQueryHandler(IGenericRepository<Governorate> repository) : IRequestHandler<GetAllGovernoratesQuery, IReadOnlyList<Governorate>>
    {
        private readonly IGenericRepository<Governorate> _repository = repository;

        public async Task<IReadOnlyList<Governorate>> Handle(GetAllGovernoratesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}

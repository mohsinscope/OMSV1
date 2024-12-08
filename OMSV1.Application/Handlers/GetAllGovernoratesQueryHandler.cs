using MediatR;
using OMSV1.Application.Queries;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers
{
    public class GetAllGovernoratesQueryHandler : IRequestHandler<GetAllGovernoratesQuery, IReadOnlyList<Governorate>>
    {
        private readonly IGenericRepository<Governorate> _repository;

        public GetAllGovernoratesQueryHandler(IGenericRepository<Governorate> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<Governorate>> Handle(GetAllGovernoratesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}

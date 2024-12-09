using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Governorates
{
    public class GetGovernorateWithOfficesQueryHandler : IRequestHandler<GetGovernorateWithOfficesQuery, Governorate>
    {
        private readonly IGenericRepository<Governorate> _repository;

        public GetGovernorateWithOfficesQueryHandler(IGenericRepository<Governorate> repository)
        {
            _repository = repository;
        }

        public async Task<Governorate?> Handle(GetGovernorateWithOfficesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdWithIncludesAsync(request.Id, g => g.Offices); // Include Offices
        }
    }
}

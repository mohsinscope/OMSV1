using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Governorates
{
    public class GetGovernorateByIdQueryHandler : IRequestHandler<GetGovernorateByIdQuery, Governorate>
    {
        private readonly IGenericRepository<Governorate> _repository;

        public GetGovernorateByIdQueryHandler(IGenericRepository<Governorate> repository)
        {
            _repository = repository;
        }

        public async Task<Governorate> Handle(GetGovernorateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}

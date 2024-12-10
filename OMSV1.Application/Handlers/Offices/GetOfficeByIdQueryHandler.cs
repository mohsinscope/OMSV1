using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Queries.Offices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Offices
{
    public class GetOfficeByIdQueryHandler : IRequestHandler<GetOfficeByIdQuery, Office>
    {
        private readonly IGenericRepository<Office> _repository;

        public GetOfficeByIdQueryHandler(IGenericRepository<Office> repository)
        {
            _repository = repository;
        }

        public async Task<Office> Handle(GetOfficeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.OfficeId);
        }
    }
}

using MediatR;
using OMSV1.Application.Queries;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers
{
    public class GetAllDamagedPassportsQueryHandler : IRequestHandler<GetAllDamagedPassportsQuery, IReadOnlyList<DamagedPassport>>
    {
        private readonly IGenericRepository<DamagedPassport> _repository;

        public GetAllDamagedPassportsQueryHandler(IGenericRepository<DamagedPassport> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<DamagedPassport>> Handle(GetAllDamagedPassportsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}

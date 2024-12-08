using MediatR;
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class GetAllDamagedPassportsQueryHandler(IGenericRepository<DamagedPassport> repository) : IRequestHandler<GetAllDamagedPassportsQuery, IReadOnlyList<DamagedPassport>>
    {
        private readonly IGenericRepository<DamagedPassport> _repository = repository;

        public async Task<IReadOnlyList<DamagedPassport>> Handle(GetAllDamagedPassportsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}

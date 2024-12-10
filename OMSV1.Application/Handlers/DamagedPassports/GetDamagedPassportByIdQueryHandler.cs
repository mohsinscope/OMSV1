using MediatR;
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class GetDamagedPassportByIdQueryHandler : IRequestHandler<GetDamagedPassportByIdQuery, DamagedPassport?>
    {
        private readonly IGenericRepository<DamagedPassport> _repository;

        public GetDamagedPassportByIdQueryHandler(IGenericRepository<DamagedPassport> repository)
        {
            _repository = repository;
        }

        public async Task<DamagedPassport?> Handle(GetDamagedPassportByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}

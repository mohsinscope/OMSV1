using MediatR;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class AddDamagedPassportCommandHandler : IRequestHandler<AddDamagedPassportCommand, int>
    {
        private readonly IGenericRepository<DamagedPassport> _repository;

        public AddDamagedPassportCommandHandler(IGenericRepository<DamagedPassport> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(AddDamagedPassportCommand request, CancellationToken cancellationToken)
        {
            var damagedPassport = new DamagedPassport(
                request.PassportNumber,
                request.Date,
                request.OfficeId,
                request.GovernorateId,
                request.DamagedTypeId
            );

            var addedEntity = await _repository.AddAsync(damagedPassport);
            return addedEntity.Id;
        }
    }
}

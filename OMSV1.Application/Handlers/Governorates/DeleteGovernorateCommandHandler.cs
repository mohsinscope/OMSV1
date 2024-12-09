using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Governorates
{
    public class DeleteGovernorateCommandHandler : IRequestHandler<DeleteGovernorateCommand, bool>
    {
        private readonly IGenericRepository<Governorate> _repository;

        public DeleteGovernorateCommandHandler(IGenericRepository<Governorate> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteGovernorateCommand request, CancellationToken cancellationToken)
        {
            var governorate = await _repository.GetByIdAsync(request.Id);
            if (governorate == null) return false;

            await _repository.DeleteAsync(governorate);
            return true;
        }
    }
}

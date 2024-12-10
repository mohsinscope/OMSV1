using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.Offices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Offices
{
    public class DeleteOfficeCommandHandler : IRequestHandler<DeleteOfficeCommand, bool>
    {
        private readonly IGenericRepository<Office> _repository;

        public DeleteOfficeCommandHandler(IGenericRepository<Office> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _repository.GetByIdAsync(request.OfficeId);
            if (office == null)
            {
                return false; // Office not found
            }

            await _repository.DeleteAsync(office);
            return true;
        }
    }
}

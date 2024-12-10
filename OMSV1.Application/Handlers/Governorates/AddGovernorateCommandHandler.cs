using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Governorates
{
    public class AddGovernorateCommandHandler : IRequestHandler<AddGovernorateCommand, int>
    {
        private readonly IGenericRepository<Governorate> _repository;

        public AddGovernorateCommandHandler(IGenericRepository<Governorate> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(AddGovernorateCommand request, CancellationToken cancellationToken)
        {
            var governorate = new Governorate(request.Name, request.Code);
            await _repository.AddAsync(governorate);
            return governorate.Id;
        }
    }
}

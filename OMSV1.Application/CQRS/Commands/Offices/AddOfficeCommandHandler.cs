using AutoMapper;
using MediatR;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Commands.Offices
{
    public class AddOfficeCommandHandler : IRequestHandler<AddOfficeCommand, int>
    {
        private readonly IGenericRepository<Office> _repository;
        private readonly IMapper _mapper;

        public AddOfficeCommandHandler(IGenericRepository<Office> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = _mapper.Map<Office>(request);
            await _repository.AddAsync(office);
            return office.Id;
        }
    }
}

using AutoMapper;
using MediatR;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Commands.Offices
{
    public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand, bool>
    {
        private readonly IGenericRepository<Office> _repository;
        private readonly IMapper _mapper;

        public UpdateOfficeCommandHandler(IGenericRepository<Office> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _repository.GetByIdAsync(request.OfficeId);
            if (office == null)
                return false;

            _mapper.Map(request, office); // Updates the entity using AutoMapper
            await _repository.UpdateAsync(office);
            return true;
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

public class AddGovernorateCommandHandler : IRequestHandler<AddGovernorateCommand, int>
{
    private readonly IGenericRepository<Governorate> _repository;
    private readonly IMapper _mapper;

    public AddGovernorateCommandHandler(IGenericRepository<Governorate> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(AddGovernorateCommand request, CancellationToken cancellationToken)
    {
        var governorate = _mapper.Map<Governorate>(request);
        await _repository.AddAsync(governorate);
        return governorate.Id;
    }
}


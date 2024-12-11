using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

public class UpdateGovernorateCommandHandler : IRequestHandler<UpdateGovernorateCommand, bool>
{
    private readonly IGenericRepository<Governorate> _repository;
    private readonly IMapper _mapper;

    public UpdateGovernorateCommandHandler(IGenericRepository<Governorate> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateGovernorateCommand request, CancellationToken cancellationToken)
    {
        var governorate = await _repository.GetByIdAsync(request.Id);
        if (governorate == null) return false;

        _mapper.Map(request, governorate);
        await _repository.UpdateAsync(governorate);
        return true;
    }
}


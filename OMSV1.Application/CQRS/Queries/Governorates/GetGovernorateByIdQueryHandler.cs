using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

public class GetGovernorateByIdQueryHandler : IRequestHandler<GetGovernorateByIdQuery, GovernorateDto>
{
    private readonly IGenericRepository<Governorate> _repository;
    private readonly IMapper _mapper;

    public GetGovernorateByIdQueryHandler(IGenericRepository<Governorate> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GovernorateDto> Handle(GetGovernorateByIdQuery request, CancellationToken cancellationToken)
    {
        var governorate = await _repository.GetByIdAsync(request.Id);
        return governorate == null ? null : _mapper.Map<GovernorateDto>(governorate);
    }
}

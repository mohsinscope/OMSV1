using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetAllGovernoratesQueryHandler : IRequestHandler<GetAllGovernoratesQuery, List<GovernorateDto>>
{
    private readonly IGenericRepository<Governorate> _repository;
    private readonly IMapper _mapper;

    public GetAllGovernoratesQueryHandler(IGenericRepository<Governorate> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<GovernorateDto>> Handle(GetAllGovernoratesQuery request, CancellationToken cancellationToken)
    {
        var governorates = await _repository.GetAllAsync();
        return _mapper.Map<List<GovernorateDto>>(governorates);
    }
}

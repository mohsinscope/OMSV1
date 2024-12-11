using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Queries.Offices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetAllOfficesQueryHandler : IRequestHandler<GetAllOfficesQuery, List<OfficeDto>>
{
    private readonly IGenericRepository<Office> _repository;
    private readonly IMapper _mapper;

    public GetAllOfficesQueryHandler(IGenericRepository<Office> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<OfficeDto>> Handle(GetAllOfficesQuery request, CancellationToken cancellationToken)
    {
        var offices = await _repository.GetAllAsync();
        return _mapper.Map<List<OfficeDto>>(offices); // AutoMapper converts entities to DTOs
    }
}

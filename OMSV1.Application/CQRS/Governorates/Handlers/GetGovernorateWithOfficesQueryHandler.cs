using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Governorates
{
   public class GetGovernorateWithOfficesQueryHandler : IRequestHandler<GetGovernorateWithOfficesQuery, GovernorateWithOfficesDto>
{
    private readonly IGenericRepository<Governorate> _repository;
    private readonly IMapper _mapper;

    public GetGovernorateWithOfficesQueryHandler(IGenericRepository<Governorate> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GovernorateWithOfficesDto> Handle(GetGovernorateWithOfficesQuery request, CancellationToken cancellationToken)
    {
        // Fetch governorate with offices included
        var governorate = await _repository.GetByIdWithIncludesAsync(request.Id, g => g.Offices);

        // Return null if not found
        if (governorate == null)
            return null;

        // Map Governorate entity to GovernorateWithOfficesDto
        return _mapper.Map<GovernorateWithOfficesDto>(governorate);
    }
}


}

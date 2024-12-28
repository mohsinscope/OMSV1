using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;

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
        try
        {
            var governorate = await _repository.GetByIdAsync(request.Id);
            if (governorate == null)
            {
                throw new Exception($"Governorate with ID {request.Id} not found.");
            }

            return _mapper.Map<GovernorateDto>(governorate);
        }
        catch (Exception ex)
        {
            // Catch and throw a custom exception for better error reporting
            throw new HandlerException("An error occurred while retrieving the governorate by ID.", ex);
        }
    }
}

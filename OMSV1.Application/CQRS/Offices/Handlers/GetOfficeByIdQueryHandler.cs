using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Offices
{
    public class GetOfficeByIdQueryHandler : IRequestHandler<GetOfficeByIdQuery, OfficeDto>
    {
        private readonly IGenericRepository<Office> _repository;
        private readonly IMapper _mapper;

        public GetOfficeByIdQueryHandler(IGenericRepository<Office> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OfficeDto> Handle(GetOfficeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var office = await _repository.GetByIdAsync(request.OfficeId);
                return office == null ? null : _mapper.Map<OfficeDto>(office);
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logging library like Serilog or NLog)
                throw new HandlerException("An error occurred while retrieving the office.", ex);
            }
        }
    }
}

using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

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
            var office = await _repository.GetByIdAsync(request.OfficeId);
            return office == null ? null : _mapper.Map<OfficeDto>(office);
        }
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.CQRS.Commands.DamagedDevices;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.DamagedDevices;

namespace OMSV1.Application.CQRS.Queries.DamagedDevices
{
    public class GetDamagedDevicesByOfficeQueryHandler 
        : IRequestHandler<GetDamagedDevicesByOfficeQuery, List<DamagedDeviceDto>>
    {
        private readonly IGenericRepository<DamagedDevice> _repository;
        private readonly IMapper _mapper;

        public GetDamagedDevicesByOfficeQueryHandler(
            IGenericRepository<DamagedDevice> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DamagedDeviceDto>> Handle(
            GetDamagedDevicesByOfficeQuery request, 
            CancellationToken cancellationToken)
        {
            var spec = new DamagedDevicesByOfficeSpecification(
                request.OfficeId, 
                request.StartDate, 
                request.EndDate, 
                request.PageNumber, 
                request.PageSize
            );

            var devices = await _repository.ListAsync(spec);
            return _mapper.Map<List<DamagedDeviceDto>>(devices);
        }
    }
}

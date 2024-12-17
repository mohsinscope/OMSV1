using System.Data.Common;
using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Queries.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class GetDamagedDeviceByIdQueryHandler : IRequestHandler<GetDamagedDeviceByIdQuery, DamagedDeviceDto?>  // Return DamagedDeviceDto
    {
        private readonly IGenericRepository<DamagedDevice> _repository;
        private readonly IMapper _mapper;

        public GetDamagedDeviceByIdQueryHandler(IGenericRepository<DamagedDevice> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DamagedDeviceDto?> Handle(GetDamagedDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch the DamagedDevice with related entities
            var damagedDevice = await _repository.GetByIdWithIncludesAsync(
                request.Id,
                dd => dd.DeviceType,     // Include DeviceType
                dd => dd.Governorate,    // Include Governorate
                dd => dd.Office,         // Include Office
                dd => dd.Profile  // Include Profile
            );

            if (damagedDevice == null)
            {
                return null;  // Return null if no DamagedDevice is found
            }

            // Map the entity to DamagedDeviceDto using AutoMapper
            var damagedDeviceDto = _mapper.Map<DamagedDeviceDto>(damagedDevice);

            return damagedDeviceDto;
        }
    }
}

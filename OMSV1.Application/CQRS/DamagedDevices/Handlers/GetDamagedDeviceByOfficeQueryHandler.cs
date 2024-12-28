using AutoMapper;
using MediatR;
using OMSV1.Application.CQRS.Commands.DamagedDevices;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Helpers;
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
            try
            {
                // Create specification for the query
                var spec = new DamagedDevicesByOfficeSpecification(
                    request.OfficeId, 
                    request.StartDate, 
                    request.EndDate, 
                    request.PageNumber, 
                    request.PageSize
                );

                // Get the list of devices based on the specification
                var devices = await _repository.ListAsync(spec);
                
                // Map the devices to DTOs
                return _mapper.Map<List<DamagedDeviceDto>>(devices);
            }
            catch (HandlerException ex)
            {
                // Log and throw the custom exception with additional details
                throw new HandlerException("An error occurred while retrieving damaged devices by office.", ex);
            }
            catch (Exception ex)
            {
                // Catch any unexpected exceptions and rethrow them as a HandlerException
                throw new HandlerException("An unexpected error occurred while retrieving damaged devices by office.", ex);
            }
        }
    }
}

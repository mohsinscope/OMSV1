using System;
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
    public class GetDamagedDevicesByGovernorateQueryHandler 
        : IRequestHandler<GetDamagedDevicesByGovernorateQuery, List<DamagedDeviceDto>>
    {
        private readonly IGenericRepository<DamagedDevice> _repository;
        private readonly IMapper _mapper;

        public GetDamagedDevicesByGovernorateQueryHandler(
            IGenericRepository<DamagedDevice> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DamagedDeviceDto>> Handle(
            GetDamagedDevicesByGovernorateQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                // Create the specification for the query
                var spec = new DamagedDevicesByGovernorateSpecification(
                    request.GovernorateId, 
                    request.StartDate, 
                    request.EndDate, 
                    pageNumber: request.PageNumber, 
                    pageSize: request.PageSize
                );

                // Get the list of devices based on the specification
                var devices = await _repository.ListAsync(spec);

                // Map the devices to DTOs
                return _mapper.Map<List<DamagedDeviceDto>>(devices);
            }
            catch (HandlerException ex)
            {
                // Catch and handle the specific business logic exceptions
                throw new HandlerException("An error occurred while retrieving damaged devices by governorate.", ex);
            }
            catch (Exception ex)
            {
                // Catch and handle unexpected errors, like database or mapping issues
                throw new HandlerException("An unexpected error occurred while retrieving damaged devices by governorate.", ex);
            }
        }
    }
}

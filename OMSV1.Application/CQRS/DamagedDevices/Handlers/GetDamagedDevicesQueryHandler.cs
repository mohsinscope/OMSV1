using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.CQRS.DamagedDevices.Queries;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.DamagedDevices;

namespace OMSV1.Application.CQRS.DamagedDevices.Commands;

public class GetDamagedDevicesQueryHandler : IRequestHandler<GetDamagedDevicesQuery, PagedList<DamagedDeviceDto>>
{
    private readonly IGenericRepository<DamagedDevice> _repository;
    private readonly IMapper _mapper;

    public GetDamagedDevicesQueryHandler(IGenericRepository<DamagedDevice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

public async Task<PagedList<DamagedDeviceDto>> Handle(GetDamagedDevicesQuery request, CancellationToken cancellationToken)
{
    // Create the specification based on the query parameters
    var spec = new FilterDamagedDevicesSpecification(
        request.SerialNumber,
        request.StartDate,
        request.EndDate,
        request.DamagedDeviceTypeId,
        request.DeviceTypeId,
        request.OfficeId,
        request.GovernorateId,
        request.ProfileId);

    // Get the queryable list of DamagedDevice entities
    var queryableResult = _repository.ListAsQueryable(spec);

    // Map to DamagedDeviceDto
    var mappedQuery = queryableResult.ProjectTo<DamagedDeviceDto>(_mapper.ConfigurationProvider);

    // Create a paginated list of DamagedDeviceDto
    return await PagedList<DamagedDeviceDto>.CreateAsync(mappedQuery, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
}

}

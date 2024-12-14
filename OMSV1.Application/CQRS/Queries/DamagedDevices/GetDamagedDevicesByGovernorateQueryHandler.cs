using System;
using AutoMapper;
using MediatR;
using OMSV1.Application.CQRS.Commands.DamagedDevices;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.DamagedDevices;

namespace OMSV1.Application.CQRS.Queries.DamagedDevices;

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
        var spec = new DamagedDevicesByGovernorateSpecification(
            request.GovernorateId, 
            request.StartDate, 
            request.EndDate, 
            pageNumber: request.PageNumber, 
            pageSize: request.PageSize
        );

        var devices = await _repository.ListAsync(spec);
        return _mapper.Map<List<DamagedDeviceDto>>(devices);
    }
}

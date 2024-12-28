using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedDevices;
using Microsoft.EntityFrameworkCore;

namespace OMSV1.Application.Commands.LOV
{
    public class GetAllDamagedDeviceTypesQueryHandler : IRequestHandler<GetAllDamagedDeviceTypesQuery, List<DamagedDeviceTypeDto>>
    {
        private readonly IGenericRepository<DamagedDeviceType> _damagedDeviceTypeRepository;

        public GetAllDamagedDeviceTypesQueryHandler(IGenericRepository<DamagedDeviceType> damagedDeviceTypeRepository)
        {
            _damagedDeviceTypeRepository = damagedDeviceTypeRepository;
        }

        public async Task<List<DamagedDeviceTypeDto>> Handle(GetAllDamagedDeviceTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch all damaged device types from the repository
                var damagedDeviceTypes = await _damagedDeviceTypeRepository.GetAllAsQueryable()
                    .Select(ddt => new DamagedDeviceTypeDto
                    {
                        Id = ddt.Id,
                        Name = ddt.Name,
                        Description = ddt.Description
                    })
                    .ToListAsync(cancellationToken);

                return damagedDeviceTypes;
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw a custom HandlerException
                throw new HandlerException("An error occurred while retrieving the damaged device types.", ex);
            }
        }
    }
}

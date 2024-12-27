using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class GetAllDamagedDevicesQueryHandler : IRequestHandler<GetAllDamagedDevicesQuery, PagedList<DamagedDeviceAllDto>>
    {
        private readonly IGenericRepository<DamagedDevice> _repository;
        private readonly IMapper _mapper;

        public GetAllDamagedDevicesQueryHandler(IGenericRepository<DamagedDevice> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DamagedDeviceAllDto>> Handle(GetAllDamagedDevicesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the damaged devices as IQueryable
                var damagedDevicesQuery = _repository.GetAllAsQueryable();
                
                // Apply ordering here - replace 'Date' with the field you want to order by
                damagedDevicesQuery = damagedDevicesQuery.OrderByDescending(dp => dp.Date);  // Example: Order by Date in descending order

                // Map to DamagedDeviceAllDto using AutoMapper's ProjectTo
                var mappedQuery = damagedDevicesQuery.ProjectTo<DamagedDeviceAllDto>(_mapper.ConfigurationProvider);

                // Apply pagination using PagedList
                var pagedDamagedDevices = await PagedList<DamagedDeviceAllDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedDamagedDevices;  // Return the paginated list
            }
            catch (HandlerException ex)
            {
                // Log and rethrow the custom exception
                throw new HandlerException("Error occurred while retrieving all damaged devices.", ex);
            }
            catch (Exception ex)
            {
                // Catch unexpected errors and rethrow them as HandlerException
                throw new HandlerException("An unexpected error occurred while retrieving all damaged devices.", ex);
            }
        }
    }
}

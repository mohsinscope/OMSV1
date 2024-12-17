using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class GetAllDamagedPassportsQueryHandler : IRequestHandler<GetAllDamagedPassportsQuery, PagedList<DamagedPassportDto>>
    {
        private readonly IGenericRepository<DamagedPassport> _repository;
        private readonly IMapper _mapper;

        public GetAllDamagedPassportsQueryHandler(IGenericRepository<DamagedPassport> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DamagedPassportDto>> Handle(GetAllDamagedPassportsQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the damaged passports as IQueryable
            var damagedPassportsQuery = _repository.GetAllAsQueryable();

            // Map to DamagedPassportDto using AutoMapper's ProjectTo
            var mappedQuery = damagedPassportsQuery.ProjectTo<DamagedPassportDto>(_mapper.ConfigurationProvider);

            // Apply pagination using PagedList
            var pagedDamagedPassports = await PagedList<DamagedPassportDto>.CreateAsync(
                mappedQuery,
                request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize
            );

            return pagedDamagedPassports;  // Return the paginated list
        }
    }
}

﻿using AutoMapper;
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
    public class GetAllDamagedPassportsQueryHandler : IRequestHandler<GetAllDamagedPassportsQuery, PagedList<DamagedPassportAllDto>>
    {
        private readonly IGenericRepository<DamagedPassport> _repository;
        private readonly IMapper _mapper;

        public GetAllDamagedPassportsQueryHandler(IGenericRepository<DamagedPassport> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DamagedPassportAllDto>> Handle(GetAllDamagedPassportsQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the damaged passports as IQueryable
            var damagedPassportsQuery = _repository.GetAllAsQueryable();
            // Apply ordering here - replace 'Date' with the field you want to order by
            damagedPassportsQuery = damagedPassportsQuery.OrderByDescending(dp => dp.Date);  // Example: Order by Date in descending order


            // Map to DamagedPassportAllDto using AutoMapper's ProjectTo
            var mappedQuery = damagedPassportsQuery.ProjectTo<DamagedPassportAllDto>(_mapper.ConfigurationProvider);

            // Apply pagination using PagedList
            var pagedDamagedPassports = await PagedList<DamagedPassportAllDto>.CreateAsync(
                mappedQuery,
                request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize

            );

            return pagedDamagedPassports;  // Return the paginated list
        }
    }
}

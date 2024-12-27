using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.DamagedPassports;
using OMSV1.Application.CQRS.DamagedPassports.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.CQRS.DamagedPassports.Handlers
{
    public class GetDamagedPassportsQueryHandler : IRequestHandler<GetDamagedPassportQuery, PagedList<DamagedPassportDto>>
    {
        private readonly IGenericRepository<DamagedPassport> _repository;
        private readonly IMapper _mapper;

        public GetDamagedPassportsQueryHandler(IGenericRepository<DamagedPassport> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DamagedPassportDto>> Handle(GetDamagedPassportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Create the specification based on the query parameters
                var spec = new FilterDamagedPassportsSpecification(
                    request.PassportNumber,
                    request.StartDate,
                    request.EndDate,
                    request.DamagedTypeId,
                    request.OfficeId,
                    request.GovernorateId,
                    request.ProfileId
                );

                // Get the queryable list of DamagedPassport entities
                var queryableResult = _repository.ListAsQueryable(spec);

                // Map to DamagedPassportDto
                var mappedQuery = queryableResult.ProjectTo<DamagedPassportDto>(_mapper.ConfigurationProvider);

                // Create a paginated list of DamagedPassportDto
                return await PagedList<DamagedPassportDto>.CreateAsync(mappedQuery, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors and throw a custom exception
                throw new HandlerException("An error occurred while retrieving damaged passports.", ex);
            }
        }
    }
}

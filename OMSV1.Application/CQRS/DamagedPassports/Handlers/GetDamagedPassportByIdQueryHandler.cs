using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class GetDamagedPassportByIdQueryHandler : IRequestHandler<GetDamagedPassportByIdQuery, DamagedPassportDto?>
    {
        private readonly IGenericRepository<DamagedPassport> _repository;
        private readonly IMapper _mapper;

        public GetDamagedPassportByIdQueryHandler(IGenericRepository<DamagedPassport> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DamagedPassportDto?> Handle(GetDamagedPassportByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the specific damaged passport as an IQueryable
                var damagedPassportQuery = _repository.GetAllAsQueryable()
                    .Where(dp => dp.Id == request.Id); // Filter by the given ID

                // Map to DamagedPassportDto using AutoMapper's ProjectTo
                var damagedPassportDto = await damagedPassportQuery
                    .ProjectTo<DamagedPassportDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken); // Fetch the first result or null

                return damagedPassportDto; // Return the mapped DTO
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors and throw a custom exception
                throw new HandlerException("An error occurred while retrieving the damaged passport by ID.", ex);
            }
        }
    }
}

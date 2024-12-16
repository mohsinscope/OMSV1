using AutoMapper;
using MediatR;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Dtos.Profiles;
// Alias the domain profile
using DomainProfile = OMSV1.Domain.Entities.Profiles.Profile;
using OMSV1.Application.Queries.Profiles;
using OMSV1.Domain.Specifications.Profiles;

namespace OMSV1.Application.CQRS.Queries.Profiles
{
    public class GetProfileByUserIdQueryHandler : IRequestHandler<GetProfileByUserIdQuery, ProfileDto>
    {
        private readonly IGenericRepository<DomainProfile> _repository;
        private readonly IMapper _mapper;

        public GetProfileByUserIdQueryHandler(IGenericRepository<DomainProfile> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProfileDto> Handle(GetProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProfileByUserIdSpecification(request.UserId);
            var profile = await _repository.SingleOrDefaultAsync(spec);

            if (profile == null)
            {
                throw new Exception($"Profile not found for User ID {request.UserId}");
            }

            return _mapper.Map<ProfileDto>(profile);
        }
    }
}
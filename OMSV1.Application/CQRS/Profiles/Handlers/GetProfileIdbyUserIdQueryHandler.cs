// using MediatR;
// using OMSV1.Domain.SeedWork;
// // Alias the domain profile
// using DomainProfile = OMSV1.Domain.Entities.Profiles.Profile;
// using OMSV1.Application.Queries.Profiles;

// namespace OMSV1.Application.CQRS.Queries.Profiles
// {
//     public class GetProfileIdByUserIdQueryHandler : IRequestHandler<GetProfileIdByUserIdQuery, int>
//     {
//         private readonly IGenericRepository<DomainProfile> _repository;

//         public GetProfileIdByUserIdQueryHandler(IGenericRepository<DomainProfile> repository)
//         {
//             _repository = repository;
//         }

//         public async Task<int> Handle(GetProfileIdByUserIdQuery request, CancellationToken cancellationToken)
//         {
//             var profile = await _repository.FirstOrDefaultAsync(p => p.UserId == request.UserId);

//             if (profile == null)
//             {
//                 throw new Exception($"Profile not found for User ID {request.UserId}");
//             }

//             return profile.Id; // Return only the Profile ID
//         }
//     }
// }

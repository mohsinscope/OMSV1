// using System;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using OMSV1.Application.Dtos.Profiles;
// using OMSV1.Infrastructure.Persistence;
// using OMSV1.Domain.Enums;
// using OMSV1.Application.Helpers;

// namespace OMSV1.Application.CQRS.Queries.Profiles
// {
//     public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ProfileDto>
//     {
//         private readonly AppDbContext _context;

//         public UpdateProfileCommandHandler(AppDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<ProfileDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
//         {
//             try
//             {
//                 // Find the profile by ID
//                 var profile = await _context.Profiles
//                     .Include(p => p.Governorate)
//                     .Include(p => p.Office)
//                     .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

//                 if (profile == null)
//                     throw new KeyNotFoundException($"Profile with ID {request.ProfileId} not found.");
                    
//                 // Update profile using the method
//                 profile.UpdateProfile(request.FullName, 
//                                       Enum.Parse<Position>(request.Position),  // Assuming Position is an enum
//                                       request.OfficeId, 
//                                       request.GovernorateId);

//                 // Save changes
//                 _context.Profiles.Update(profile);
//                 await _context.SaveChangesAsync(cancellationToken);

//                 // Map to DTO
//                 var dto = new ProfileDto
//                 {
//                     FullName = profile.FullName,
//                     Position = profile.Position.ToString(),
//                     GovernorateId = profile.GovernorateId,
//                     GovernorateName = profile.Governorate?.Name,
//                     OfficeId = profile.OfficeId,
//                     OfficeName = profile.Office?.Name
//                 };

//                 return dto;
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception (optional: implement a logging service here)
//                 throw new HandlerException("An error occurred while updating the profile.", ex);
//             }
//         }
//     }
// }

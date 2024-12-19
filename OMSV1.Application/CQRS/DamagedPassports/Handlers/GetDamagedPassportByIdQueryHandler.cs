using MediatR;
using OMSV1.Application.Dtos;  // Import the DTOs namespace
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;  // Assuming a repository interface is being used

namespace OMSV1.Application.Handlers.DamagedPassports
{
        public class GetDamagedPassportByIdQueryHandler : IRequestHandler<GetDamagedPassportByIdQuery, DamagedPassportDto>
        {
            private readonly AppDbContext _dbContext;

            public GetDamagedPassportByIdQueryHandler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<DamagedPassportDto> Handle(GetDamagedPassportByIdQuery request, CancellationToken cancellationToken)
            {
                // Ensure Attachments are included when querying the DamagedPassport
                var damagedPassport = await _dbContext.DamagedPassports
                    .Include(dp => dp.Attachments)   // Eagerly load Attachments
                    .Include(dp => dp.DamagedType)   // Eagerly load DamagedType
                    .Include(dp => dp.Office)        // Eagerly load Office
                    .Include(dp => dp.Governorate)   // Eagerly load Governorate
                    .Include(dp => dp.Profile)       // Eagerly load Profile
                    .FirstOrDefaultAsync(dp => dp.Id == request.Id, cancellationToken);

                if (damagedPassport == null)
                    throw new KeyNotFoundException($"Damaged Passport with ID {request.Id} not found.");

                // Map the retrieved entity to the DTO
                var dto = new DamagedPassportDto(
                    damagedPassport.PassportNumber,
                    damagedPassport.Date,
                    damagedPassport.DamagedTypeId,
                    damagedPassport.DamagedType?.Name,  // Safely access related DamagedType
                    damagedPassport.OfficeId,
                    damagedPassport.Office?.Name,        // Safely access related Office
                    damagedPassport.GovernorateId,
                    damagedPassport.Governorate?.Name,   // Safely access related Governorate
                    damagedPassport.ProfileId,
                    damagedPassport.Profile?.FullName,   // Safely access related Profile
                    damagedPassport.Attachments
                        .Select(a => new DamagedPassportAttachmentDto
                        {
                            FileName = a.FileName,
                            FilePath = a.FilePath // Map FilePath (URL) from AttachmentCU
                        })
                        .ToList()
                );

                return dto;
            }
        }




}

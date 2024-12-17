using MediatR;
using OMSV1.Application.Dtos;  // Import the DTOs namespace
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Entities.DamagedPassport;  // Assuming a repository interface is being used

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class GetDamagedPassportByIdQueryHandler : IRequestHandler<GetDamagedPassportByIdQuery, DamagedPassportDto?>
    {
        private readonly IGenericRepository<DamagedPassport> _repository;

        public GetDamagedPassportByIdQueryHandler(IGenericRepository<DamagedPassport> repository)
        {
            _repository = repository;
        }

        public async Task<DamagedPassportDto?> Handle(GetDamagedPassportByIdQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the damaged passport by ID from the repository
            var damagedPassport = await _repository.GetByIdAsync(request.Id);

            if (damagedPassport == null) return null;

            // Map the domain entity to the DTO
            var damagedPassportDto = new DamagedPassportDto(
                damagedPassport.PassportNumber,
                damagedPassport.Date,
                damagedPassport.DamagedTypeId,
                damagedPassport.DamagedType?.Name,
                damagedPassport.OfficeId,
                damagedPassport.Office?.Name,
                damagedPassport.GovernorateId,
                damagedPassport.Governorate?.Name,
                damagedPassport.ProfileId,
                damagedPassport.Profile?.FullName,
                damagedPassport.Attachments.Select(a => new DamagedPassportAttachmentDto
                {
                    FileName = a.FileName,
                    FilePath = a.FilePath
                }).ToList()
            );

            return damagedPassportDto;
        }
    }
}

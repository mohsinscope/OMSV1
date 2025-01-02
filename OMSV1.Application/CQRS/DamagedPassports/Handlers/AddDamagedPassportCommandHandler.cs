using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class AddDamagedPassportCommandHandler : IRequestHandler<AddDamagedPassportCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDamagedPassportCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddDamagedPassportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Validate if the OfficeId belongs to the GovernorateId using FirstOrDefaultAsync
                var office = await _unitOfWork.Repository<Office>()
                    .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);

                if (office == null)
                {
                    // If the office doesn't belong to the governorate, throw an exception
                    throw new HandlerException($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
                }

                // Step 2: Map the command to the DamagedPassport entity
                var damagedPassport = _mapper.Map<DamagedPassport>(request);

                // Step 3: Convert Date to UTC if needed
                damagedPassport.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

                // Step 4: Add the damaged passport entity to the repository
                await _unitOfWork.Repository<DamagedPassport>().AddAsync(damagedPassport);

                // Step 5: Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new HandlerException("Failed to save the damaged passport to the database.");
                }

                // Step 6: Return the ID of the newly created damaged passport
                return damagedPassport.Id;
            }
            catch (HandlerException ex)
            {
                // Custom exception handling for known issues
                throw new HandlerException($"Error in AddDamagedPassportCommandHandler: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Catching unexpected errors and wrapping them in a custom exception
                throw new HandlerException("An unexpected error occurred while adding the damaged passport.", ex);
            }
        }
    }
}

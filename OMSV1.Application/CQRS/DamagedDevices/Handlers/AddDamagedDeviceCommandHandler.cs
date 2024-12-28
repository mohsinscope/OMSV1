using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class AddDamagedDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddDamagedDeviceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

public async Task<int> Handle(AddDamagedDeviceCommand request, CancellationToken cancellationToken)
    {
        try{
        // Validate if the OfficeId belongs to the GovernorateId using AnyAsync
        var officeBelongsToGovernorate = await _unitOfWork.Repository<Office>()
        .AnyAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId, cancellationToken);


        if (!officeBelongsToGovernorate)
        {
            // If the validation fails, throw an exception
            throw new Exception($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
        }

        // Map the request to the DamagedDevice entity
        var damagedDevice = _mapper.Map<DamagedDevice>(request);

        // Convert Date to UTC before saving
        damagedDevice.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

        // Add the damaged device to the repository using AddAsync
        await _unitOfWork.Repository<DamagedDevice>().AddAsync(damagedDevice);

        // Save changes to the database
        if (!await _unitOfWork.SaveAsync(cancellationToken))
        {
            // Handle failure to save
            throw new Exception("Failed to save the damaged device to the database.");
        }

        // Return the ID of the newly created damaged device
        return damagedDevice.Id;
    }
          catch (HandlerException ex)
            {
                // Handle specific business exceptions (HandlerException)
                throw new HandlerException("An error occurred while processing the DamagedD Device creation request.", ex);
            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions (generic)
                throw new HandlerException("An unexpected error occurred.", ex);
            }
    }



    }
}

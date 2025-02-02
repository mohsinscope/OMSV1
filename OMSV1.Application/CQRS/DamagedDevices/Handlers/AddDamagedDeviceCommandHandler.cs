using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
// Handler class
public class AddDamagedDeviceCommandHandler : IRequestHandler<AddDamagedDeviceCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddDamagedDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddDamagedDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check for duplicate serial number
            var existingDevice = await _unitOfWork.Repository<DamagedDevice>()
                .FirstOrDefaultAsync(dd => dd.SerialNumber == request.SerialNumber);

            if (existingDevice != null)
            {
                throw new DuplicateDeviceException($"A damaged device with serial number {request.SerialNumber} already exists.");
            }

            // Validate if the OfficeId belongs to the GovernorateId
            var officeBelongsToGovernorate = await _unitOfWork.Repository<Office>()
                .AnyAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId, cancellationToken);

            if (!officeBelongsToGovernorate)
            {
                throw new HandlerException($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
            }

            var damagedDevice = _mapper.Map<DamagedDevice>(request);
            damagedDevice.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

            await _unitOfWork.Repository<DamagedDevice>().AddAsync(damagedDevice);

            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new HandlerException("Failed to save the damaged device to the database.");
            }

            return damagedDevice.Id;
        }
        catch (DuplicateDeviceException)
        {
            // Re-throw duplicate device exceptions to be handled by the controller
            throw;
        }
        catch (HandlerException ex)
        {
            throw new HandlerException("An error occurred while processing the Damaged Device creation request.", ex);
        }
        catch (Exception ex)
        {
            throw new HandlerException("An unexpected error occurred.", ex);
        }
    }
}

// Custom Exception class (add this to your project)
public class DuplicateDeviceException : Exception
{
    public DuplicateDeviceException(string message) : base(message) { }
    public DuplicateDeviceException(string message, Exception innerException) : base(message, innerException) { }
}

}

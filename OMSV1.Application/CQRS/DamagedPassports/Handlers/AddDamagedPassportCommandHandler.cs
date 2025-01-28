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
            // First check for duplicate passport number
            var existingPassport = await _unitOfWork.Repository<DamagedPassport>()
                .FirstOrDefaultAsync(dp => dp.PassportNumber == request.PassportNumber);

            if (existingPassport != null)
            {
                throw new DuplicatePassportException($"A damaged passport with number {request.PassportNumber} already exists.");
            }

            // Rest of your existing validation and processing
            var office = await _unitOfWork.Repository<Office>()
                .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);

            if (office == null)
            {
                throw new HandlerException($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
            }

            var damagedPassport = _mapper.Map<DamagedPassport>(request);
            damagedPassport.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));
            
            await _unitOfWork.Repository<DamagedPassport>().AddAsync(damagedPassport);

            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new HandlerException("Failed to save the damaged passport to the database.");
            }

            return damagedPassport.Id;
        }
        catch (DuplicatePassportException)
        {
            // Re-throw duplicate passport exceptions to be handled by the controller
            throw;
        }
        catch (HandlerException ex)
        {
            throw new HandlerException($"Error in AddDamagedPassportCommandHandler: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new HandlerException("An unexpected error occurred while adding the damaged passport.", ex);
        }
    }
}

// Custom Exception class (add this to your project)
public class DuplicatePassportException : Exception
{
    public DuplicatePassportException(string message) : base(message) { }
    public DuplicatePassportException(string message, Exception innerException) : base(message, innerException) { }
}

}

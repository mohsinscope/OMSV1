using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

public class UpdateGovernorateCommandHandler : IRequestHandler<UpdateGovernorateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGovernorateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateGovernorateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the governorate using the unit of work
            var governorate = await _unitOfWork.Repository<Governorate>().GetByIdAsync(request.Id);
            if (governorate == null) return false; // Governorate not found

            // Map the updated data onto the existing governorate entity
            _mapper.Map(request, governorate);

            // Update the governorate entity
            await _unitOfWork.Repository<Governorate>().UpdateAsync(governorate);

            // Save the changes via the unit of work
            if (await _unitOfWork.SaveAsync(cancellationToken))
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            // Handle the exception and throw a custom HandlerException
            throw new HandlerException("An error occurred while updating the governorate.", ex);
        }
    }
}

using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;  // Assuming HandlerException is defined in this namespace

public class AddGovernorateCommandHandler : IRequestHandler<AddGovernorateCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddGovernorateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddGovernorateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var governorate = _mapper.Map<Governorate>(request);

            // Use the UnitOfWork to add the governorate
            await _unitOfWork.Repository<Governorate>().AddAsync(governorate);

            // Save changes using UnitOfWork
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to save the governorate to the database.");
            }

            return governorate.Id;  // Return the ID of the newly created governorate
        }
        catch (Exception ex)
        {
            // Catch any exceptions and throw a custom exception for handling
            throw new HandlerException("An error occurred while adding the governorate.", ex);
        }
    }
}

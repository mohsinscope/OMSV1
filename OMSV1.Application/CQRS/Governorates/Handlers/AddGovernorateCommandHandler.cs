using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

public class AddGovernorateCommandHandler : IRequestHandler<AddGovernorateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddGovernorateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(AddGovernorateCommand request, CancellationToken cancellationToken)
    {
        var governorate = _mapper.Map<Governorate>(request);

        // Use the UnitOfWork to add the governorate
        await _unitOfWork.Repository<Governorate>().AddAsync(governorate);

        // Save changes using UnitOfWork
        if (!await _unitOfWork.SaveAsync(cancellationToken))
        {
            throw new Exception("Failed to save the governorate to the database.");
        }

        return governorate.Id;
    }
}

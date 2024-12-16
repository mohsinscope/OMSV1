using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class AddDamagedPassportCommandHandler : IRequestHandler<AddDamagedPassportCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDamagedPassportCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

public async Task<int> Handle(AddDamagedPassportCommand request, CancellationToken cancellationToken)
{
    // Map the command to the entity
    var damagedPassport = _mapper.Map<DamagedPassport>(request);

    // Convert Date to UTC before saving
    damagedPassport.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

    // Save the entity using UnitOfWork
    await _unitOfWork.Repository<DamagedPassport>().AddAsync(damagedPassport);

    if (!await _unitOfWork.SaveAsync(cancellationToken))
    {
        throw new Exception("Failed to save the damaged Passport to the database.");
    }

    return damagedPassport.Id;
}


    }
}

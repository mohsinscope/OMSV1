using AutoMapper;
using MediatR;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Commands.Offices
{
    public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork; // Inject IUnitOfWork
        private readonly IMapper _mapper;

        public UpdateOfficeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            // Fetch the office using the IUnitOfWork repository method
            var office = await _unitOfWork.Repository<Office>().GetByIdAsync(request.OfficeId);
            if (office == null)
                return false;

            // Map the properties from the request to the office entity
            _mapper.Map(request, office);

            // Use IUnitOfWork to update the entity and save the changes
            await _unitOfWork.Repository<Office>().UpdateAsync(office);

            // Save the changes
            return await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}

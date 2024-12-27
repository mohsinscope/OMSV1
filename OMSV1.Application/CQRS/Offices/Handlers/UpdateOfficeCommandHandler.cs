using AutoMapper;
using MediatR;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System;
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
            try
            {
                // Fetch the office using the IUnitOfWork repository method
                var office = await _unitOfWork.Repository<Office>().GetByIdAsync(request.OfficeId);
                if (office == null)
                {
                    // Return false if the office is not found
                    return false;
                }

                // Map the properties from the request to the office entity
                _mapper.Map(request, office);

                // Use IUnitOfWork to update the entity and save the changes
                await _unitOfWork.Repository<Office>().UpdateAsync(office);

                // Save the changes
                return await _unitOfWork.SaveAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log the error (You can use a logging library like Serilog or NLog here)
                throw new HandlerException("An error occurred while updating the office.", ex);
            }
        }
    }
}

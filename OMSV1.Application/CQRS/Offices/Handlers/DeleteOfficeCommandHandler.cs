using MediatR;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.Commands.Offices
{
    public class DeleteOfficeCommandHandler : IRequestHandler<DeleteOfficeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork; // Inject IUnitOfWork

        public DeleteOfficeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the office using the IUnitOfWork repository method
                var office = await _unitOfWork.Repository<Office>().GetByIdAsync(request.OfficeId);
                if (office == null)
                {
                    return false; // Return false if the office does not exist
                }

                // Use IUnitOfWork to delete the office
                await _unitOfWork.Repository<Office>().DeleteAsync(office);

                // Save changes to the database using IUnitOfWork
                return await _unitOfWork.SaveAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log and throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while deleting the office.", ex);
            }
        }
    }
}

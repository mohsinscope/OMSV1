using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.Offices;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Offices
{
    public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand, bool>
    {
        private readonly IGenericRepository<Office> _repository;

        public UpdateOfficeCommandHandler(IGenericRepository<Office> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _repository.GetByIdAsync(request.OfficeId);
            if (office == null)
            {
                return false; // Office not found
            }

            // Update office details
            office.UpdateName(request.Name);
            office.UpdateCode(request.Code);
            office.UpdateStaff(
                request.ReceivingStaff,
                request.AccountStaff,
                request.PrintingStaff,
                request.QualityStaff,
                request.DeliveryStaff
            );

            await _repository.UpdateAsync(office);
            return true;
        }
    }
}

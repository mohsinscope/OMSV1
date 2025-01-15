using AutoMapper;
using MediatR;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.Commands.Offices
{
    public class AddOfficeCommandHandler : IRequestHandler<AddOfficeCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddOfficeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddOfficeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Request Budget: {request.Budget}"); // Log the value

                var office = new Office(
                    request.Name,
                    request.Code,
                    request.ReceivingStaff,
                    request.AccountStaff,
                    request.PrintingStaff,
                    request.QualityStaff,
                    request.DeliveryStaff,
                    request.GovernorateId,
                    request.Budget // Explicitly map Budget
                );

                Console.WriteLine($"Office Budget: {office.Budget}"); // Log the mapped value

                await _unitOfWork.Repository<Office>().AddAsync(office);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the office to the database.");
                }

                return office.Id;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while adding the office.", ex);
            }
        }

    }
}

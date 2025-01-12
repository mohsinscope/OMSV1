using MediatR;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Commands.Thresholds
{
    public class AddThresholdCommandHandler : IRequestHandler<AddThresholdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddThresholdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddThresholdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var threshold = new Threshold(request.Name, request.MinValue, request.MaxValue);

                // Use the unit of work's repository to add the new threshold
                await _unitOfWork.Repository<Threshold>().AddAsync(threshold);
                await _unitOfWork.SaveAsync(cancellationToken); // Commit the transaction

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while adding the threshold.", ex);
            }
        }
    }
}

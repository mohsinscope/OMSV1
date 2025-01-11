using MediatR;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Commands.Thresholds
{
    public class UpdateThresholdCommandHandler : IRequestHandler<UpdateThresholdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateThresholdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateThresholdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var threshold = await _unitOfWork.Repository<Threshold>().GetByIdAsync(request.Id);
                if (threshold == null) return false;

                threshold.Update(request.Name, request.MinValue, request.MaxValue);

                await _unitOfWork.Repository<Threshold>().UpdateAsync(threshold);
                await _unitOfWork.SaveAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while updating the threshold.", ex);
            }
        }
    }
}

using MediatR;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Commands.Thresholds
{
    public class DeleteThresholdCommandHandler : IRequestHandler<DeleteThresholdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteThresholdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteThresholdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var threshold = await _unitOfWork.Repository<Threshold>().GetByIdAsync(request.Id);
                if (threshold == null) return false;

                await _unitOfWork.Repository<Threshold>().DeleteAsync(threshold);
                await _unitOfWork.SaveAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the threshold.", ex);
            }
        }
    }
}

using MediatR;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Thresholds
{
    public class GetThresholdByIdQueryHandler : IRequestHandler<GetThresholdByIdQuery, ThresholdDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetThresholdByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ThresholdDto> Handle(GetThresholdByIdQuery request, CancellationToken cancellationToken)
        {
            var threshold = await _unitOfWork.Repository<Threshold>().GetByIdAsync(request.Id);
            if (threshold == null)
            {
                throw new KeyNotFoundException($"Threshold with ID {request.Id} was not found.");
            }

            return new ThresholdDto
            {
                Id = threshold.Id,
                Name = threshold.Name,
                MinValue = threshold.MinValue,
                MaxValue = threshold.MaxValue
            };
        }
    }

}

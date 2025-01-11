using MediatR;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Thresholds
{
    public class GetThresholdsHandler : IRequestHandler<GetThresholdsQuery, List<ThresholdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetThresholdsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ThresholdDto>> Handle(GetThresholdsQuery request, CancellationToken cancellationToken)
        {
            // Fetch all thresholds using the Unit of Work
            var thresholds = await _unitOfWork.Repository<Threshold>().GetAllAsync();

            // Map the thresholds to DTOs
            return thresholds.Select(t => new ThresholdDto
            {
                Id = t.Id,
                Name = t.Name,
                MinValue = t.MinValue,
                MaxValue = t.MaxValue
            }).ToList();
        }
    }
}

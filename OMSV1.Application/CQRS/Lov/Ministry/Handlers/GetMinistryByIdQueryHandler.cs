using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Ministries
{
    public class GetMinistryByIdQueryHandler : IRequestHandler<GetMinistryByIdQuery, MinistryDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMinistryByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MinistryDto> Handle(GetMinistryByIdQuery request, CancellationToken cancellationToken)
        {
            var ministry = await _unitOfWork.Repository<Ministry>()
                .GetByIdAsync(request.Id);

            if (ministry == null)
                throw new KeyNotFoundException($"Ministry with ID {request.Id} was not found.");

            return new MinistryDto
            {
                Id = ministry.Id,
                Name = ministry.Name,
                DateCreated = ministry.DateCreated
            };
        }
    }
}


using AutoMapper;
using MediatR;
using OMSV1.Application.DTOs.Reports;
using OMSV1.Application.Queries.Reports;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Reports
{
    public class GetAllReportTypesQueryHandler : IRequestHandler<GetAllReportTypesQuery, List<ReportTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllReportTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ReportTypeDto>> Handle(GetAllReportTypesQuery request, CancellationToken cancellationToken)
        {
            // Fetch all ReportType entities from the repository.
            var reportTypes = await _unitOfWork.Repository<ReportType>().ListAllAsync();

            // Map the domain entities to DTOs using AutoMapper.
            var dtos = _mapper.Map<List<ReportTypeDto>>(reportTypes);

            return dtos;
        }
    }
}

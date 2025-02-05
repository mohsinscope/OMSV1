using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Reports;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Reports
{
    public class AddReportTypeCommandHandler : IRequestHandler<AddReportTypeCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddReportTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddReportTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Check for duplicate Name
                var existingReportType = await _unitOfWork.Repository<ReportType>()
                    .FirstOrDefaultAsync(rt => rt.Name == request.Name);

                if (existingReportType != null)
                {
                    throw new Exception($"ReportType with name '{request.Name}' already exists.");
                }

                // Step 2: Create ReportType entity
                var reportType = new ReportType(request.Name, request.Description);

                // Step 3: Add entity to the repository
                await _unitOfWork.Repository<ReportType>().AddAsync(reportType);

                // Step 4: Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the report type to the database.");
                }

                // Step 5: Return the ID of the newly created report type
                return reportType.Id;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while adding the report type.", ex);
            }
        }
    }
}

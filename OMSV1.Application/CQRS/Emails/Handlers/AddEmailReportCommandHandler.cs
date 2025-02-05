using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Reports;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Reports
{
    public class AddEmailReportCommandHandler : IRequestHandler<AddEmailReportCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddEmailReportCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddEmailReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Validate if ReportTypeIds exist
                var validReportTypes = new List<ReportType>();
                foreach (var reportTypeId in request.ReportTypeIds)
                {
                    var reportType = await _unitOfWork.Repository<ReportType>()
                        .FirstOrDefaultAsync(rt => rt.Id == reportTypeId);

                    if (reportType == null)
                    {
                        throw new Exception($"ReportType ID {reportTypeId} does not exist.");
                    }
                    
                    validReportTypes.Add(reportType);
                }

                // Step 2: Create EmailReport entity
                var emailReport = new EmailReport(request.FullName, request.Email);

                // Step 3: Add ReportTypes to the EmailReport
                foreach (var reportType in validReportTypes)
                {
                    emailReport.AddReportType(reportType);
                }

                // Step 4: Add EmailReport entity to the repository
                await _unitOfWork.Repository<EmailReport>().AddAsync(emailReport);

                // Step 5: Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the email report to the database.");
                }

                // Step 6: Return the ID of the newly created email report
                return emailReport.Id;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while adding the email report.", ex);
            }
        }
    }
}

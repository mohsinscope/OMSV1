using MediatR;
using OMSV1.Application.Commands.Reports;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Reports
{
    public class UpdateEmailReportCommandHandler : IRequestHandler<UpdateEmailReportCommand, bool>
    {
        private readonly IGenericRepository<EmailReport> _emailReportRepository;
        private readonly IGenericRepository<ReportType> _reportTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmailReportCommandHandler(
            IGenericRepository<EmailReport> emailReportRepository,
            IGenericRepository<ReportType> reportTypeRepository,
            IUnitOfWork unitOfWork)
        {
            _emailReportRepository = emailReportRepository;
            _reportTypeRepository = reportTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateEmailReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the existing EmailReport including its ReportTypes
                var emailReport = await _emailReportRepository.GetByIdWithIncludesAsync(
                    request.Id,
                    er => er.ReportTypes);
                    
                if (emailReport == null)
                    throw new KeyNotFoundException($"EmailReport with ID {request.Id} not found.");

                // Update basic properties using a domain method
                emailReport.UpdateEmailReport(request.FullName, request.Email);

                // Remove all existing report type associations
                foreach (var existingReportType in emailReport.ReportTypes.ToList())
                {
                    emailReport.RemoveReportType(existingReportType);
                }

                // Add new report type associations
                foreach (var reportTypeId in request.ReportTypeIds)
                {
                    var reportType = await _reportTypeRepository.GetByIdAsync(reportTypeId);
                    if (reportType == null)
                        throw new KeyNotFoundException($"ReportType with ID {reportTypeId} not found.");

                    emailReport.AddReportType(reportType);
                }

                // Update the EmailReport asynchronously
                await _emailReportRepository.UpdateAsync(emailReport);

                // Commit changes via the unit of work
                return await _unitOfWork.SaveAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while updating the email report.", ex);
            }
        }
    }
}
